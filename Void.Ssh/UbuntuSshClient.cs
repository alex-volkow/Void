using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using Void.IO;

namespace Void.Net
{
    public class UbuntuSshClient : BasicClient
    {
        public override bool IsAdmin => throw new NotImplementedException();

        public override FilePath UserFolder => throw new NotImplementedException();



        public UbuntuSshClient(ConnectionInfo seed) 
            : base(seed) {
        }

        public UbuntuSshClient(string host, string username, string password) 
            : base(host, username, password) {
        }

        public UbuntuSshClient(string host, string username, params PrivateKeyFile[] keys) 
            : base(host, username, keys) {
        }

        public UbuntuSshClient(string host, int port, string username, string password) 
            : base(host, port, username, password) {
        }

        public UbuntuSshClient(string host, int port, string username, params PrivateKeyFile[] keys) 
            : base(host, port, username, keys) {
        }



        public override bool Delete(FilePath path) {
            throw new NotImplementedException();
        }

        public override bool Exists(FilePath path) {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<string>> GetServices() {
            throw new NotImplementedException();
        }

        public override Task<string> GetSha256Async(FilePath path) {
            throw new NotImplementedException();
        }

        public override Task<string> GetSha512Async(FilePath path) {
            throw new NotImplementedException();
        }

        public override Task OpenInTcpPortAsync(int port) {
            throw new NotImplementedException();
        }

        public override SftpFileStream OpenRead(FilePath path) {
            throw new NotImplementedException();
        }

        public override Task RestartService(string service) {
            throw new NotImplementedException();
        }

        public override Task StartService(string service) {
            throw new NotImplementedException();
        }

        public override Task StopService(string service) {
            throw new NotImplementedException();
        }

        public override Task WriteAsync(FilePath path, Stream stream) {
            throw new NotImplementedException();
        }
    }
}
