using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.IO;

namespace Void.Net
{
    public class LinuxAdapter : SshAdapter
    {
        public LinuxAdapter(SshClient client) 
            : base(client) {
        }


        public virtual Task<string> GetSha256Async(FilePath path) {
            return GetShaAsync(path, 256);
        }

        public virtual Task<string> GetSha512Async(FilePath path) {
            return GetShaAsync(path, 512);
        }

        public virtual async Task<FilePath> GetUserFolderAsync() {
            return (await ExecuteAsync("eval echo ~$USER")).Trim();
        }

        private async Task<string> GetShaAsync(FilePath path, int value) {
            if (path == null) {
                throw new ArgumentNullException(nameof(path));
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
    }
}
