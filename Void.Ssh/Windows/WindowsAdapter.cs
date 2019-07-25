using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Renci.SshNet;
using Void.IO;

namespace Void.Net
{
    public class WindowsAdapter : SshAdapter
    {
        private static readonly Regex SHA_PARSER = new Regex(@"\w{32,}");



        public WindowsAdapter(SshClient client) 
            : base(client) {
        }

        

        public virtual async Task<bool> IsAdminAsync() {
            await ExecuteAsync("net session >nul 2>&1");
            var result = (await ExecuteAsync("echo %errorLevel%")).Trim();
            if (int.TryParse(result, out int code)) {
                return code == 0;
            }
            throw new InvalidOperationException(
                $"Invalid code: {result}"
                );
        }

        public virtual async Task<FilePath> GetUserFolderAsync() {
            return (await ExecuteAsync("echo %userprofile%"))
                .Remove("\r")
                .Remove("\n");
        }

        public virtual async Task RestartServiceAsync(string service) {
            await StopServiceAsync(service);
            await StartServiceAsync(service);
        }

        public virtual async Task StartServiceAsync(string service) {
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

        public virtual async Task StopServiceAsync(string service) {
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

        public virtual async Task<IEnumerable<string>> GetServicesAsync() {
            var result = await ExecuteAsync($"sc queryex type= service state= all");
            return Regex
                .Matches(result, @"SERVICE_NAME:\s*(?<NAME>[^\s\r\n]+)")
                .Cast<Match>()
                .Select(e => e.Groups["NAME"].Value)
                .OrderBy(e => e)
                .ToArray();
        }

        public virtual async Task InstallServiceAsync(IRemoteServiceInfo service) {
            if (service == null) {
                throw new ArgumentNullException(nameof(service));
            }
            if (string.IsNullOrWhiteSpace(service.Name)) {
                throw new ArgumentException("Value must not be empty",
                    $"{nameof(service)}.{nameof(IRemoteServiceInfo.Name)}"
                    );
            }
            if (string.IsNullOrWhiteSpace(service.File)) {
                throw new ArgumentException("Value must not be empty",
                    $"{nameof(service)}.{nameof(IRemoteServiceInfo.File)}"
                    );
            }
            var services = await GetServicesAsync();
            if (services.Any(e => e == service.Name)) {
                throw new InvalidOperationException(
                    $"Service is already installed"
                    );
            }
            await ExecuteAsync($@"SC CREATE ""{service.Name}"" start= auto binpath= ""{service.File}""");
        }

        public virtual async Task UnistallServiceAsync(string service) {
            if (string.IsNullOrWhiteSpace(service)) {
                throw new ArgumentException("Value must not be empty",
                    nameof(service)
                    );
            }
            var services = await GetServicesAsync();
            if (!services.Any(e => e == service)) {
                throw new InvalidOperationException(
                    $"Service is not installed"
                    );
            }
            await StopServiceAsync(service);
            await ExecuteAsync($@"SC DELETE ""{service}""");
        }

        public virtual Task<string> GetSha256Async(FilePath path) {
            return GetShaAsync(path, "SHA256");
        }

        public virtual Task<string> GetSha512Async(FilePath path) {
            return GetShaAsync(path, "SHA512");
        }

        public virtual Task OpenInTcpPortAsync(int port) {
            var command = new StringBuilder();
            command.Append($@"netsh advfirewall firewall add rule").Append(" ");
            command.Append($@"name=""TCP IN ").Append(port).Append($@"""").Append(" ");
            command.Append($@"dir=in action=allow protocol=TCP localport=").Append(port);
            return ExecuteAsync(command.ToString());
        }

        public virtual async Task<IEnumerable<FilePath>> GetFilesRecursiveAsync(FilePath path) {
            var result = await ExecuteAsync($"dir \"{path.ToWindows()}\"/a-d /b /s");
            return result
                .Split('\n')
                .Select(e => e.Trim())
                .Where(e => !string.IsNullOrEmpty(e))
                .Select(e => new FilePath(e));
        }

        private async Task<string> GetShaAsync(FilePath path, string value) {
            if (path == null) {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(value)) {
                throw new ArgumentException(
                    "Value mustnot be empty",
                    nameof(value)
                    );
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
