using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.IO;

namespace Void.Net
{
    public abstract class BasicSshClient : IDisposable
    {
        public static TimeSpan DefaultConnectionInterval { get; } = TimeSpan.FromSeconds(5);


        private readonly List<BaseClient> reconnecting;
        private readonly object locker;
        private TimeSpan connectionInterval;
        private bool autoconnect;


        public abstract bool IsAdmin { get; }

        public abstract FilePath UserFolder { get; }
        
        public bool IsAutoConnect {
            get {
                lock (this.locker) {
                    return this.autoconnect;
                }
            }
        }

        public TimeSpan ConnectionInterval {
            get {
                lock (this.locker) {
                    return this.connectionInterval;
                }
            }
        }


        protected SshClient Shell { get; }

        protected SftpClient Sftp { get; }



        public BasicSshClient(ConnectionInfo seed) {
            if (seed == null) {
                throw new ArgumentNullException(nameof(seed));
            }
            this.Shell = new SshClient(seed);
            this.Sftp = new SftpClient(seed);
            this.reconnecting = new List<BaseClient>();
            this.locker = new object();
        }



        public abstract Task<string> GetSha256Async(FilePath path);

        public abstract Task<string> GetSha512Async(FilePath path);

        public abstract Task OpenInTcpPortAsync(int port);

        public virtual Task WriteAsync(FilePath path, Stream stream) {
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

        public async Task WriteAsync(FilePath path, byte[] content) {
            using (var stream = new MemoryStream(content)) {
                await WriteAsync(path, stream); 
            }
        }

        public Task WriteAsync(FilePath path, string content) => WriteAsync(path, content, Encoding.UTF8);

        public Task WriteAsync(FilePath path, string content, Encoding encoding) {
            if (encoding == null) {
                throw new ArgumentNullException(nameof(encoding));
            }
            return WriteAsync(path, encoding.GetBytes(content));
        }

        public async Task<byte[]> ReadBytesAsync(FilePath path) {
            using (var input = OpenRead(path))
            using (var output = new MemoryStream()) {
                await input.CopyToAsync(output);
                return output.ToArray();
            }
        }

        public Task<string> ReadTextAsync(FilePath path) => ReadTextAsync(path, Encoding.UTF8);

        public async Task<string> ReadTextAsync(FilePath path, Encoding encoding) {
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

        public virtual bool Exists(FilePath path) {
            return this.Sftp.Exists(path?.ToUnix());
        }

        public virtual bool Delete(FilePath path) {
            path = path.ToUnix();
            if (!Exists(path)) {
                return false;
            }
            foreach (var file in this.Sftp.ListDirectory(path)) {
                if ((file.Name != ".") && (file.Name != "..")) {
                    if (file.IsDirectory) {
                        Delete(file.FullName);
                    }
                    else {
                        this.Sftp.DeleteFile(file.FullName);
                    }
                }
            }
            this.Sftp.DeleteDirectory(path);
            return true;
        }

        public abstract Task RestartService(string service);

        public abstract Task StartService(string service);

        public abstract Task StopService(string service);

        public abstract Task<IEnumerable<string>> GetServices();

        public abstract Task InstallService(IRemoteServiceInfo service);

        public abstract Task UnistallService(string service);

        public virtual async Task<bool> IsDifferent(FilePath local, FilePath remote) {
            var localExists = File.Exists(local ?? throw new ArgumentNullException(nameof(local)));
            var remoteExists = Exists(remote ?? throw new ArgumentNullException(nameof(remote)));
            if (!localExists && !remoteExists) {
                return false;
            }
            if (!localExists || !remoteExists) {
                return true;
            }
            using (var stream = File.Open(local, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                var remoteHash = GetSha512Async(remote);
                var localHash = stream.GetSHA512();
                return localHash != await remoteHash;
            }
        }

        public async Task<string> ExecuteAsync(string command) {
            var handler = this.Shell.CreateCommand(command);
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

        public string Execute(string command) {
            var handler = this.Shell.RunCommand(command);
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

        public virtual void Connect() {
            var methods = new Action[] {
                this.Shell.Connect,
                this.Sftp.Connect
            };
            methods.AsParallel().ForAll(e => e.Invoke());
        }

        public virtual void Dispose() {
            var methods = new Action[] {
                this.Shell.Dispose,
                this.Sftp.Dispose
            };
            Disconnect();
            methods.AsParallel().ForAll(e => e.Invoke());
        }

        public void AutoConnect() {
            AutoConnect(DefaultConnectionInterval);
        }

        public void AutoConnect(TimeSpan interval) {
            lock (this.locker) {
                this.connectionInterval = interval.Duration();
                if (!this.IsAutoConnect) {
                    this.autoconnect = true;
                    this.Shell.ErrorOccurred += HandleError;
                    this.Sftp.ErrorOccurred += HandleError;
                }
            }
            TryConnect(this.Shell);
            TryConnect(this.Sftp);
        }

        public void Disconnect() {
            lock (this.locker) {
                if (this.autoconnect) {
                    this.autoconnect = false;
                    this.Shell.ErrorOccurred -= HandleError;
                    this.Sftp.ErrorOccurred -= HandleError;
                }
            }
            if (this.Shell.IsConnected) {
                this.Shell.Disconnect();
            }
            if (this.Sftp.IsConnected) {
                this.Sftp.Disconnect();
            }
        }

        public override string ToString() {
            return this.Shell.ConnectionInfo.Port != 22
                ? $"{this.Shell.ConnectionInfo.Host}:{this.Shell.ConnectionInfo.Port}"
                : this.Shell.ConnectionInfo.Host;
        }

        protected virtual void Reconnect(BaseClient client) {
            client.Connect();
        }

        private void HandleError(object sender, ExceptionEventArgs e) {
            TryConnect((BaseClient)sender); //SshAuthenticationException
        }

        private async void TryConnect(BaseClient client) {
            lock (this.locker) {
                if (this.reconnecting.Contains(client)) {
                    return;
                }
                this.reconnecting.Add(client);
            }
            try {
                while (!client.IsConnected && this.IsAutoConnect) {
                    try {
                        Reconnect(client);
                    }
                    catch (Exception ex) {
                        if (ex is ObjectDisposedException) {
                            Disconnect();
                            return;
                        }
                        await Task.Delay(this.ConnectionInterval);
                    }
                }
            }
            catch { }
            finally {
                lock (this.locker) {
                    this.reconnecting.Remove(client);
                }
            }
        }
    }
}
