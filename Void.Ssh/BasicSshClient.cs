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
    public abstract class BasicSshClient : IDisposable
    {
        private readonly object locker;


        public abstract bool IsAdmin { get; }

        public abstract FilePath UserFolder { get; }


        protected SshClient Shell { get; }

        protected SftpClient Sftp { get; }



        public BasicSshClient(ConnectionInfo seed) {
            if (seed == null) {
                throw new ArgumentNullException(nameof(seed));
            }
            this.Shell = new SshClient(seed);
            this.Sftp = new SftpClient(seed);
            this.locker = new object();
        }

        public BasicSshClient(string host, string username, string password) 
            : this(host, 22, username, password) {
        }

        public BasicSshClient(string host, int port, string username, string password) {
            this.Shell = new SshClient(host, port, username, password);
            this.Sftp = new SftpClient(host, port, username, password);
            this.locker = new object();
        }

        public BasicSshClient(string host, string username, params PrivateKeyFile[] keys)
            : this(host, 22, username, keys) {
        }

        public BasicSshClient(string host, int port, string username, params PrivateKeyFile[] keys) {
            this.Shell = new SshClient(host, port, username, keys);
            this.Sftp = new SftpClient(host, port, username, keys);
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

        //public virtual async Task InstallService(string service, FilePath executable) {
        //    var services = await GetServices();
        //    if (services.Any(e => e == service)) {
        //        throw new InvalidOperationException(
        //            $"Service is already installed"
        //            );
        //    }
        //    if (this.Session.Exists(installingDirectory)) {
        //        if (Terminal.ReadYesNo("Remote directory is not empty. Delete it? (y/n) > ")) {
        //            Terminal.Write($"Removing '{installingDirectory}'...");
        //            this.Session.Delete(installingDirectory);
        //            Terminal.WriteLine("\tOK");
        //        }
        //    }
        //    Terminal.Write($"Registering service '");
        //    Terminal.Write(this.ServiceName, ConsoleColor.White);
        //    Terminal.Write("'...");
        //    var executable = Files.Combine(installingDirectory, GetEntryPointFileName());
        //    var result = this.Session.Execute($@"SC CREATE ""{this.ServiceName}"" start= auto binpath= ""dotnet \""{executable}\"" --service""");
        //    Terminal.WriteLine("\tOK");
        //    if (Terminal.ReadYesNo("Start service? (y/n) > ")) {
        //        await StandupService();
        //    }
        //    Terminal.WriteLine();
        //}


        //public virtual async Task UnistallService() {
        //    Terminal.Write("Checking remote services...");
        //    if (!GetWindowsServices().Any(e => e == this.ServiceName)) {
        //        Terminal.WriteLine("\tOK");
        //        Terminal.WriteLine("Service is not installed", ConsoleColor.Yellow);
        //        Terminal.WriteLine();
        //        return;
        //    }
        //    Terminal.WriteLine("\tOK");
        //    Terminal.WriteLine("Service found", ConsoleColor.Green);
        //    await ShutdownService();
        //    Terminal.Write("Removing service...");
        //    var result = this.Session.Execute($@"SC DELETE ""{this.ServiceName}""");
        //    Terminal.WriteLine("\tOK");
        //    var installingDirectory = CombineRemote(string.Empty);
        //    Terminal.Write($"Installation folder: ");
        //    Terminal.WriteLine(installingDirectory, ConsoleColor.White);
        //    if (this.Session.Exists(installingDirectory)) {
        //        Terminal.Write("Removing binaries...");
        //        this.Session.Delete(installingDirectory);
        //        Terminal.WriteLine("\tOK");
        //    }
        //    else {
        //        Terminal.WriteLine("No binaries found", ConsoleColor.Yellow);
        //    }
        //    Terminal.WriteLine("Done", ConsoleColor.Green);
        //}

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
