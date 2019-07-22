using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using Void.IO;

namespace Void.Net
{
    public abstract class LinuxSshClient : BasicSshClient
    {
        private readonly Lazy<string> userFolder;


        public override FilePath UserFolder => this.userFolder.Value;



        public LinuxSshClient(ConnectionInfo seed) 
            : base(seed) {
            this.userFolder = new Lazy<string>(GetUserFolder);
        }



        public override Task<string> GetSha256Async(FilePath path) {
            return GetShaAsync(path, 256);
        }

        public override Task<string> GetSha512Async(FilePath path) {
            return GetShaAsync(path, 512);
        }

        private async Task<string> GetShaAsync(FilePath path, int value) {
            if (!this.Sftp.Exists(path)) {
                return null;
            }
            if (!this.Sftp.Get(path).IsRegularFile) {
                return null;
            }
            var result = await ExecuteAsync($"sha{value}sum \"{path}\"");
            var parts = result.Split(' ').Where(e => !string.IsNullOrWhiteSpace(e));
            if (parts.Count() != 2) {
                throw new InvalidDataException(
                    $"Invalid result: {result}"
                    );
            }
            return parts.First();
        }

        private string GetUserFolder() {
            return Execute("eval echo ~$USER").Trim();
        }
    }
}
