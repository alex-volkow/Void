using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Void.IO;

namespace Void.Net
{
    public abstract class BaseManager : IDisposable
    {
        public static int DefaultSshPoolSize { get; } = Environment.ProcessorCount;

        public static int DefaultSftpPoolSize { get; } = Environment.ProcessorCount;



        private readonly List<Session<SshClient>> sshSessions;
        private readonly List<Session<SftpClient>> sftpSessions;
        private readonly object locker;



        public ConnectionInfo ConnectionInfo { get; }

        public int SshPoolSize {
            get {
                lock (this.locker) {
                    return this.sshSessions.Capacity;
                }
            }
        }

        public int SftpPoolSize {
            get {
                lock (this.locker) {
                    return this.sftpSessions.Capacity;
                }
            }
        }

        public int SshSessionsCount {
            get {
                lock (this.locker) {
                    return this.sshSessions.Count;
                }
            }
        }

        public int SftpSessionsCount {
            get {
                lock (this.locker) {
                    return this.sftpSessions.Count;
                }
            }
        }

        public abstract bool IsAdmin { get; }

        public abstract FilePath UserFolder { get; }



        public BaseManager(ConnectionInfo settings, int sshPoolSize, int sftpPoolSize) {
            this.ConnectionInfo = settings ?? throw new ArgumentNullException(nameof(settings));
            this.sftpSessions = new List<Session<SftpClient>>(Math.Max(sftpPoolSize, 1));
            this.sshSessions = new List<Session<SshClient>>(Math.Max(sshPoolSize, 1));
            this.locker = new object();
        }



        public abstract Task RestartService(string service, CancellationToken token);

        public abstract Task StartService(string service, CancellationToken token);

        public abstract Task StopService(string service, CancellationToken token);

        public abstract Task<IEnumerable<string>> GetServices(CancellationToken token);

        public abstract Task InstallService(IRemoteServiceInfo service, CancellationToken token);

        public abstract Task UnistallService(string service, CancellationToken token);

        public abstract Task<string> GetSha256Async(FilePath path, CancellationToken token);

        public abstract Task<string> GetSha512Async(FilePath path, CancellationToken token);

        public abstract Task OpenInTcpPortAsync(int port, CancellationToken token);

        public virtual Task WriteAsync(FilePath path, Stream stream, CancellationToken token) {
            path = path.ToUnix();
            var files = path
                .ToString()
                .Split('/')
                .Where(e => !string.IsNullOrEmpty(e))
                .ToArray();
            var locator = string.Empty;
            for (var i = 0; i < (files.Length - 1); i++) {
                locator += $"/{files[i]}";
                lock (this.locker) {
                    if (!Exists(locator)) {
                        this.Sftp.CreateDirectory(locator);
                    }
                }
            }
            return Task.Factory.FromAsync(
                this.Sftp.BeginUploadFile(stream, path),
                this.Sftp.EndUploadFile
                );
        }

        public async Task WriteAsync(FilePath path, byte[] content, CancellationToken token) {
            using (var stream = new MemoryStream(content)) {
                await WriteAsync(path, stream);
            }
        }

        public Task WriteAsync(FilePath path, string content) => WriteAsync(path, content, Encoding.UTF8, CancellationToken token);

        public Task WriteAsync(FilePath path, string content, Encoding encoding) {
            if (encoding == null) {
                throw new ArgumentNullException(nameof(encoding));
            }
            return WriteAsync(path, encoding.GetBytes(content));
        }

        public async Task<byte[]> ReadBytesAsync(FilePath path, CancellationToken token) {
            using (var input = OpenRead(path))
            using (var output = new MemoryStream()) {
                await input.CopyToAsync(output);
                return output.ToArray();
            }
        }

        public Task<string> ReadTextAsync(FilePath path, CancellationToken token) => ReadTextAsync(path, Encoding.UTF8, token);

        public async Task<string> ReadTextAsync(FilePath path, Encoding encoding, CancellationToken token) {
            if (encoding == null) {
                throw new ArgumentNullException(nameof(encoding));
            }
            var bytes = await ReadBytesAsync(path);
            return encoding.GetString(bytes);
        }

        public virtual SftpFileStream OpenRead(FilePath path) {
            return this.Sftp.OpenRead(path?.ToUnix());
        }

        public virtual IEnumerable<FilePath> GetFiles(FilePath path) {
            if (Exists(path)) {
                var file = this.Sftp.Get(path);
                if (file.IsRegularFile) {
                    return new FilePath[] { };
                }
                return this.Sftp.ListDirectory(path)
                    .Where(e => e.Name != "." && e.Name != "..")
                    .Select(e => (FilePath)e.FullName);
            }
            return new FilePath[] { };
        }

        public virtual IEnumerable<FilePath> GetFilesRecursive(FilePath path) {
            if (Exists(path)) {
                var file = this.Sftp.Get(path);
                if (file.IsRegularFile) {
                    return new FilePath[] { };
                }
                var content = this.Sftp.ListDirectory(path)
                    .Where(e => e.Name != "." && e.Name != "..")
                    .Select(e => (FilePath)e.FullName);
                var files = new List<FilePath>(content);
                files.AddRange(content.AsParallel().SelectMany(e => GetFilesRecursive(e)));
                return files;
            }
            return new FilePath[] { };
        }

        public Task<bool> Exists(FilePath path) {
            return Exists(path, CancellationToken.None);
        }

        public async Task<bool> Exists(FilePath path, CancellationToken token) {
            using (var session = await GetSftpSession(token)) {
                return session.Client.Exists(path);
            }
        }

        public Task<bool> Delete(FilePath path) {
            return Delete(path, CancellationToken.None);
        }

        public async Task<bool> Delete(FilePath path, CancellationToken token) {
            using (var session = await GetSftpSession(token)) {
                path = path.ToUnix();
                if (!session.Client.Exists(path)) {
                    return false;
                }
                foreach (var file in session.Client.Client.ListDirectory(path)) {
                    if (!token.IsCancellationRequested && (file.Name != ".") && (file.Name != "..")) {
                        if (file.IsDirectory) {
                            await DeleteAsync(client, file.FullName, token);
                        }
                        else {
                            client.DeleteFile(file.FullName);
                        }
                    }
                }
                client.DeleteDirectory(path);
                return true;
            }
        }

        public virtual Task<bool> IsDifferent(FilePath local, FilePath remote) {
            if (local == null) {
                throw new ArgumentNullException(nameof(local));
            }
            return IsDifferent(new FileInfo(local), remote);
        }

        public virtual async Task<bool> IsDifferent(FileInfo local, FilePath remote) {
            if (local == null) {
                throw new ArgumentNullException(nameof(local));
            }
            var localExists = File.Exists(local.FullName);
            var remoteExists = Exists(remote ?? throw new ArgumentNullException(nameof(remote)));
            if (!localExists && !remoteExists) {
                return false;
            }
            if (!localExists || !remoteExists) {
                return true;
            }
            using (var stream = local.Open(FileMode.Open, FileAccess.Read, FileShare.Read)) {
                var remoteHash = GetSha512Async(remote);
                var localHash = stream.GetSHA512();
                return localHash != await remoteHash;
            }
        }

        public virtual async Task<bool> IsDifferent(Stream stream, FilePath remote) {
            if (stream == null) {
                throw new ArgumentNullException(nameof(stream));
            }
            if (!Exists(remote ?? throw new ArgumentNullException(nameof(remote)))) {
                return true;
            }
            var remoteHash = GetSha512Async(remote);
            var localHash = stream.GetSHA512();
            return localHash != await remoteHash;
        }

        public Task<string> ExecuteAsync(string command) {
            return ExecuteAsync(command, CancellationToken.None);
        }

        public async Task<string> ExecuteAsync(string command, CancellationToken token) {
            using (var session = await GetSession(this.sshSessions, token)) {
                return await ExecuteAsync(session.Driver.Client, command, token);
            }                
        }

        public virtual void Dispose() {
            throw new NotImplementedException();
        }

        public virtual void Free() {
            lock (this.locker) {
                var sshSessions = this.sshSessions.Where(e => !e.IsActive).ToArray();
                foreach (var session in sshSessions) {
                    using (session)
                    using (session.Client) {
                        this.sshSessions.Remove(session);
                    }
                }
                var sftpSessions = this.sftpSessions.Where(e => !e.IsActive).ToArray();
                foreach (var session in sftpSessions) {
                    using (session)
                    using (session.Client) {
                        this.sftpSessions.Remove(session);
                    }
                }
            }
        }

        

        protected async Task<string> ExecuteAsync(SshClient client, string command, CancellationToken token) {
            var handler = client.CreateCommand(command);
            await Task.Factory.FromAsync(handler.BeginExecute(), handler.EndExecute);
            if (handler.ExitStatus != 0) {
                var error = handler.Error;
                if (string.IsNullOrWhiteSpace(error)) {
                    error = handler.Result;
                }
                throw new InvalidOperationException(
                    $"[Code {handler.ExitStatus}] {error?.Trim()}"
                    );
            }
            return handler.Result;
        }

        protected async Task<bool> DeleteAsync(ISession<SftpClient> session, FilePath path, CancellationToken token) {
            path = path.ToUnix();
            if (!session.Client.Exists(path)) {
                return false;
            }
            foreach (var file in session.Client.ListDirectory(path)) {
                if (!token.IsCancellationRequested && (file.Name != ".") && (file.Name != "..")) {
                    if (file.IsDirectory) {
                        await DeleteAsync(session, file.FullName, token);
                    }
                    else {
                        session.Client.DeleteFile(file.FullName);
                    }
                }
            }
            session.Client.DeleteDirectory(path);
            return true;
        }

        protected async Task<ISession<SshClient>> GetSshSession(CancellationToken token) {
            return await GetSession(this.sshSessions, token);
        }

        protected async Task<ISession<SftpClient>> GetSftpSession(CancellationToken token) {
            return await GetSession(this.sftpSessions, token);
        }

        private async Task<Session<T>> GetSession<T>(List<Session<T>> pool, CancellationToken token) where T : BaseClient {
            lock (this.locker) {
                if (pool.Count < pool.Capacity) {
                    var client = (T)Activator.CreateInstance(typeof(T), this.ConnectionInfo);
                    var session = new Session<T>(client);
                    session.Client.Client.Connect();
                    session.Client.AutoConnect();
                    pool.Add(session);
                    session.Activate();
                    return session;
                }
            }
            while (!token.IsCancellationRequested) {
                var sessions = GetSessions<T>(pool);
                var awaiters = sessions.Select(e => e.GetAwaiter()).ToArray();
                await Task.WhenAny(awaiters);
                lock (this.locker) {
                    var session = sessions.FirstOrDefault(e => !e.IsActive);
                    if (session != null) {
                        session.Activate();
                        return session;
                    }
                }
            }
            throw new OperationCanceledException();
        }

        private IReadOnlyList<Session<T>> GetSessions<T>(List<Session<T>> pool) where T : BaseClient {
            lock (this.locker) {
                return pool.ToArray();
            }
        }
    }
}
