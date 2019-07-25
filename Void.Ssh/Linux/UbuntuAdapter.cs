using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Renci.SshNet;

namespace Void.Net
{
    public class UbuntuAdapter : LinuxAdapter
    {
        private static readonly Regex SYSTEMCTL_SERVICES_PARSER = new Regex(
            @"^[^\S\r\n]*(?<NAME>[^\s]+)[^\S\r\n]+(?<LOAD>[^\s]+)[^\S\r\n]+(?<ACTIVE>[^\s]+)[^\S\r\n]+(?<SUB>[^\s]+)[^\S\r\n]+(?<DESCRIPTION>[^\r\n]+)$"
        );


        public UbuntuAdapter(SshClient client) 
            : base(client) {
        }


        public virtual async Task<IEnumerable<string>> GetServicesAsync() {
            var result = await ExecuteAsync("systemctl --full --type service --all --no-pager");
            return SYSTEMCTL_SERVICES_PARSER
                .Matches(result)
                .Cast<Match>()
                .Select(e => e.Groups["NAME"].Value.Trim())
                .ToArray();
        }

        public virtual Task OpenInTcpPortAsync(int port) {
            return ExecuteAsync($"sudo ufw allow {port}/tcp");
        }

        public virtual Task RestartServiceAsync(string service) {
            return ExecuteAsync($"sudo systemctl restart {service}");
        }

        public virtual Task StartServiceAsync(string service) {
            return ExecuteAsync($"sudo systemctl start {service}");
        }

        public virtual Task StopServiceAsync(string service) {
            return ExecuteAsync($"sudo systemctl stop {service}");
        }

        public async Task<IEnumerable<string>> GetAllUsersAsync() {
            return (await ExecuteAsync("awk -F':' '{ print $1}' /etc/passwd"))
                .Split('\n')
                .Select(e => e.Trim())
                .Where(e => !string.IsNullOrEmpty(e))
                .OrderBy(e => e)
                .ToArray();
        }

        public async Task<IEnumerable<string>> GetSudoUsersAsync() {
            return (await ExecuteAsync("grep '^sudo:.*$' /etc/group | cut -d: -f4"))
                .Split('\n')
                .Select(e => e.Trim())
                .Where(e => !string.IsNullOrEmpty(e))
                .OrderBy(e => e)
                .ToArray();
        }

        //public string CreateService(string name, string executable, string user, string description) {
        //    // https://askubuntu.com/questions/465877/how-to-move-one-file-to-a-folder-using-terminal
        //    // sudo chown -R user:user /path (gran user rights to write)
        //    var exe = this.sftp.Get(executable);
        //    var directoryExtractor = new Regex($@"^(?<NAME>.+)/{exe.Name}&");
        //    var directory = directoryExtractor.Match(executable);
        //    var content = new StringBuilder();
        //    content.Append("[Unit]\n");
        //    content.Append("Description=").Append(description).Append('\n');
        //    content.Append("\n");
        //    content.Append("[Service]\n");
        //    content.Append("WorkingDirectory=").Append(directory.Groups["NAME"].Value).Append('\n');
        //    if (executable.EndsWith(".dll")) {
        //        content.Append("ExecStart=/usr/bin/dotnet ").Append(executable).Append('\n');
        //    }
        //    else {
        //        content.Append("ExecStart=").Append(executable).Append('\n');
        //    }
        //    content.Append("Restart=always\n");
        //    content.Append("RestartSec=10\n");
        //    content.Append("KillSignal=SIGINT\n");
        //    content.Append("SyslogIdentifier=").Append(exe.Name.Remove(" ")).Append("-log").Append('\n');
        //    content.Append("User=").Append(user).Append('\n');
        //    content.Append("Environment=ASPNETCORE_ENVIRONMENT=Production\n");
        //    content.Append("Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false\n");
        //    content.Append("\n");
        //    content.Append("[Install]\n");
        //    content.Append("WantedBy=multi-user.target\n");
        //    if (!name.EndsWith(".service")) {
        //        name += ".service";
        //    }
        //    var path = $"/etc/systemd/system/{name}";
        //    using (var stream = new MemoryStream()) {
        //        var bytes = Encoding.UTF8.GetBytes(content.ToString());
        //        stream.Write(bytes, 0, bytes.Length);
        //        var command = this.shell.RunCommand($"sudo touch {path}");
        //        if (command.ExitStatus != 0) {
        //            throw new InvalidOperationException(
        //                $"[Code {command.ExitStatus}] {command.Error}"
        //                );
        //        }
        //        this.sftp.UploadFile(stream, path);
        //        return path;
        //    }
        //}
    }
}
