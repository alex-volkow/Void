using Renci.SshNet;
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
    public abstract class BasicClient : IDisposable
    {
        public abstract bool IsAdmin { get; }

        public abstract FilePath UserFolder { get; }


        protected SshClient Shell { get; }

        protected SftpClient Sftp { get; }



        public BasicClient(ConnectionInfo seed) {
            if (seed == null) {
                throw new ArgumentNullException(nameof(seed));
            }
            this.Shell = new SshClient(seed);
            this.Sftp = new SftpClient(seed);
        }

        public BasicClient(string host, string username, string password) 
            : this(host, 22, username, password) {
        }

        public BasicClient(string host, int port, string username, string password) {
            this.Shell = new SshClient(host, port, username, password);
            this.Sftp = new SftpClient(host, port, username, password);
        }

        public BasicClient(string host, string username, params PrivateKeyFile[] keys)
            : this(host, 22, username, keys) {
        }

        public BasicClient(string host, int port, string username, params PrivateKeyFile[] keys) {
            this.Shell = new SshClient(host, port, username, keys);
            this.Sftp = new SftpClient(host, port, username, keys);
        }



        public abstract Task<string> GetSha256Async(FilePath path);

        public abstract Task<string> GetSha512Async(FilePath path);

        public abstract Task OpenInTcpPortAsync(int port);

        public abstract Task WriteAsync(FilePath path, Stream stream);

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

        public abstract SftpFileStream OpenRead(FilePath path);

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

        public abstract bool Exists(FilePath path);

        public abstract bool Delete(FilePath path);

        public abstract Task RestartService(string service);

        public abstract Task StartService(string service);

        public abstract Task StopService(string service);

        public abstract Task<IEnumerable<string>> GetServices();

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
                    $"[Code {handler.ExitStatus}] {error}"
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
                    $"[Code {handler.ExitStatus}] {error}"
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
            methods.AsParallel().ForAll(e => e.Invoke());
        }

        public override string ToString() {
            return this.Shell.ConnectionInfo.Port != 22
                ? $"{this.Shell.ConnectionInfo.Host}:{this.Shell.ConnectionInfo.Port}"
                : this.Shell.ConnectionInfo.Host;
        }
    }
}
