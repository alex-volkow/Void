using System;
using System.Collections.Generic;
using System.Text;
using Renci.SshNet;

namespace Void.Net
{
    public abstract class LinuxSshClient : BasicClient
    {
        public LinuxSshClient(ConnectionInfo seed) 
            : base(seed) {
        }

        public LinuxSshClient(string host, string username, string password) 
            : base(host, username, password) {
        }

        public LinuxSshClient(string host, string username, params PrivateKeyFile[] keys) 
            : base(host, username, keys) {
        }

        public LinuxSshClient(string host, int port, string username, string password) 
            : base(host, port, username, password) {
        }

        public LinuxSshClient(string host, int port, string username, params PrivateKeyFile[] keys) 
            : base(host, port, username, keys) {
        }
    }
}
