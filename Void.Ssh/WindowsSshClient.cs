using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using Void.IO;

namespace Void.Net
{
    public class WindowsSshClient : BasicSshClient
    {
        private static readonly Regex SHA_PARSER = new Regex(@"\w{32,}");

        private readonly Lazy<FilePath> userFolder;
        private readonly Lazy<bool> isAdmin;


        public override bool IsAdmin => this.isAdmin.Value;

        public override FilePath UserFolder => this.userFolder.Value;


        public WindowsSshClient(ConnectionInfo seed) 
            : base(seed) {
            this.userFolder = new Lazy<FilePath>(GetRemoteUserDirectory);
            this.isAdmin = new Lazy<bool>(CheckAdminRights);
        }

        public WindowsSshClient(string host, string username, string password) 
            : base(host, username, password) {
            this.userFolder = new Lazy<FilePath>(GetRemoteUserDirectory);
            this.isAdmin = new Lazy<bool>(CheckAdminRights);
        }

        public WindowsSshClient(string host, string username, params PrivateKeyFile[] keys) 
            : base(host, username, keys) {
            this.userFolder = new Lazy<FilePath>(GetRemoteUserDirectory);
            this.isAdmin = new Lazy<bool>(CheckAdminRights);
        }

        public WindowsSshClient(string host, int port, string username, string password) 
            : base(host, port, username, password) {
            this.userFolder = new Lazy<FilePath>(GetRemoteUserDirectory);
            this.isAdmin = new Lazy<bool>(CheckAdminRights);
        }

        public WindowsSshClient(string host, int port, string username, params PrivateKeyFile[] keys) 
            : base(host, port, username, keys) {
            this.userFolder = new Lazy<FilePath>(GetRemoteUserDirectory);
            this.isAdmin = new Lazy<bool>(CheckAdminRights);
        }



        public override async Task RestartService(string service) {
            await StopService(service);
            await StartService(service);
        }

        public override async Task StartService(string service) {
            if (service == null) {
                throw new ArgumentNullException(nameof(service));
            }
            try {
                await ExecuteAsync($@"SC START ""{service}""");
            }
            catch (InvalidOperationException ex) {
                if (!ex.Message.Contains("service is already running")) {
                    throw;
                }
            }
        }

        public override async Task StopService(string service) {
            if (service == null) {
                throw new ArgumentNullException(nameof(service));
            }
            try {
                await ExecuteAsync($@"SC STOP ""{service}""");
            }
            catch (InvalidOperationException ex) {
                if (!ex.Message.Contains("service has not been started")) {
                    throw;
                }
            }
        }

        public override async Task<IEnumerable<string>> GetServices() {
            var result = await ExecuteAsync($"sc queryex type= service state= all");
            return Regex
                .Matches(result, @"SERVICE_NAME:\s*(?<NAME>[^\s\r\n]+)")
                .Cast<Match>()
                .Select(e => e.Groups["NAME"].Value)
                .OrderBy(e => e)
                .ToArray();
        }

        public override async Task InstallService(IRemoteServiceInfo service) {
            if (service == null) {
                throw new ArgumentNullException(nameof(service));
            }
            if (string.IsNullOrWhiteSpace(service.Name)) {
                throw new ArgumentException("Vlaue must not be empty",
                    $"{nameof(service)}.{nameof(IRemoteServiceInfo.Name)}"
                    );
            }
            if (string.IsNullOrWhiteSpace(service.File)) {
                throw new ArgumentException("Vlaue must not be empty",
                    $"{nameof(service)}.{nameof(IRemoteServiceInfo.File)}"
                    );
            }
            if (!Exists(service.File)) {
                throw new InvalidDataException(
                    $"Remote file does not exist"
                    );
            }
            var services = await GetServices();
            if (services.Any(e => e == service.Name)) {
                throw new InvalidOperationException(
                    $"Service is already installed"
                    );
            }
            await ExecuteAsync($@"SC CREATE ""{service.Name}"" start= auto binpath= ""{service.File}""");
        }

        public override async Task UnistallService(string service) {
            if (string.IsNullOrWhiteSpace(service)) {
                throw new ArgumentException("Value must not be empty",
                    nameof(service)
                    );
            }
            var services = await GetServices();
            if (!services.Any(e => e == service)) {
                throw new InvalidOperationException(
                    $"Service is not installed"
                    );
            }
            await StopService(service);
            Terminal.Write("Removing service...");
            await ExecuteAsync($@"SC DELETE ""{service}""");
        }

        public override Task<string> GetSha256Async(FilePath path) {
            return GetShaAsync(path, "SHA256");
        }

        public override Task<string> GetSha512Async(FilePath path) {
            return GetShaAsync(path, "SHA512");
        }

        public override Task OpenInTcpPortAsync(int port) {
            var command = new StringBuilder();
            command.Append($@"netsh advfirewall firewall add rule").Append(" ");
            command.Append($@"name=""TCP IN ").Append(port).Append($@"""").Append(" ");
            command.Append($@"dir=in action=allow protocol=TCP localport=").Append(port);
            return ExecuteAsync(command.ToString());
        }

        public override IEnumerable<FilePath> GetFilesRecursive(FilePath path) {
            return Execute($"dir \"{path.ToWindows()}\"/a-d /b /s")
                .Split('\n')
                .Select(e => e.Trim())
                .Where(e => !string.IsNullOrEmpty(e))
                .Select(e => new FilePath(e));
        }

        private bool CheckAdminRights() {
            Execute("net session >nul 2>&1");
            var result = Execute("echo %errorLevel%").Trim();
            if (int.TryParse(result, out int code)) {
                return code == 0;
            }
            throw new InvalidOperationException(
                $"Invalid code: {result}"
                );
        }

        private FilePath GetRemoteUserDirectory() {
            return Execute("echo %userprofile%")
                .Remove("\r")
                .Remove("\n");
        }

        private async Task<string> GetShaAsync(FilePath path, string value) {
            if (path == null) {
                throw new ArgumentNullException(nameof(path));
            }
            if (!Exists(path)) {
                return null;
            }
            if (!this.Sftp.Get(path).IsRegularFile) {
                return null;
            }
            var result = await ExecuteAsync($@"certUtil -hashfile ""{path.ToWindows()}"" {value}");
            var match = SHA_PARSER.Match(result);
            if (!match.Success) {
                throw new InvalidDataException(
                    $"Invalid result: {result}"
                    );
            }
            return match.Value;
        }
    }
}
