﻿using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Net
{
    public abstract class BasicClient : IDisposable
    {
        private readonly Lazy<SshClient> shell;
        private readonly Lazy<SftpClient> sftp;



        protected SshClient Shell => this.shell.Value;

        protected SftpClient Sftp => this.sftp.Value;



        public BasicClient(ConnectionInfo seed) {
            if (seed == null) {
                throw new ArgumentNullException(nameof(seed));
            }
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
    }
}