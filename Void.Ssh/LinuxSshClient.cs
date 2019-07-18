using System;
using System.Collections.Generic;
using System.Text;
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

        public LinuxSshClient(string host, string username, string password) 
            : base(host, username, password) {
            this.userFolder = new Lazy<string>(GetUserFolder);
        }

        public LinuxSshClient(string host, string username, params PrivateKeyFile[] keys) 
            : base(host, username, keys) {
            this.userFolder = new Lazy<string>(GetUserFolder);
        }

        public LinuxSshClient(string host, int port, string username, string password) 
            : base(host, port, username, password) {
            this.userFolder = new Lazy<string>(GetUserFolder);
        }

        public LinuxSshClient(string host, int port, string username, params PrivateKeyFile[] keys) 
            : base(host, port, username, keys) {
            this.userFolder = new Lazy<string>(GetUserFolder);
        }


        private string GetUserFolder() {
            return Execute("eval echo ~$USER").Trim();
        }
    }
}
