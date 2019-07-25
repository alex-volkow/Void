using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Void.IO;

namespace Void.Net
{
    public class SshAdapter : Adapter<SshClient>
    {
        public SshAdapter(SshClient client) 
            : base(client) {
        }
        
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

        protected override void Reconnect() {
            base.Reconnect();
            var ports = this.Client.ForwardedPorts.ToArray();
            foreach (var port in ports) {
                port.Stop();
                this.Client.RemoveForwardedPort(port);
                this.Client.AddForwardedPort(port);
                port.Start();
            }
        }
    }
}
