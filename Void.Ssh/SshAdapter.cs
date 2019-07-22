using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Void.IO;

namespace Void.Net
{
    public abstract class SshAdapter : Adapter<SshClient>
    {
        public SshAdapter(SshClient client) 
            : base(client) {
        }
        


        public abstract Task<bool> IsAdminAsync();

        public abstract Task<FilePath> GetUserFolderAsync();

        public abstract Task<string> GetSha256Async(FilePath path);

        public abstract Task<string> GetSha512Async(FilePath path);

        public abstract Task RestartService(string service);

        public abstract Task StartService(string service);

        public abstract Task StopService(string service);

        public abstract Task<IEnumerable<string>> GetServices();

        public virtual async Task<string> ExecuteAsync(string command) {
            var handler = this.Client.CreateCommand(command);
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
    }
}
