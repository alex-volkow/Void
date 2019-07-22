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
    public class SftpAdapter : Adapter<SftpClient>
    {
        public SftpAdapter(SftpClient client) 
            : base(client) {
        }


        public virtual IEnumerable<FilePath> GetFilesRecursive(FilePath path) {
            if (Exists(path)) {
                var file = this.Client.Get(path);
                if (file.IsRegularFile) {
                    return new FilePath[] { };
                }
                var content = this.Client.ListDirectory(path)
                    .Where(e => e.Name != "." && e.Name != "..")
                    .Select(e => (FilePath)e.FullName);
                var files = new List<FilePath>(content);
                files.AddRange(content.AsParallel().SelectMany(e => GetFilesRecursive(e)));
                return files;
            }
            return new FilePath[] { };
        }

        public virtual bool Exists(FilePath path) {
            return this.Client.Exists(path?.ToUnix());
        }

        public virtual bool Delete(FilePath path) {
            path = path.ToUnix();
            if (!Exists(path)) {
                return false;
            }
            foreach (var file in this.Client.ListDirectory(path)) {
                if ((file.Name != ".") && (file.Name != "..")) {
                    if (file.IsDirectory) {
                        Delete(file.FullName);
                    }
                    else {
                        this.Client.DeleteFile(file.FullName);
                    }
                }
            }
            this.Client.DeleteDirectory(path);
            return true;
        }

        public virtual IEnumerable<FilePath> GetFiles(FilePath path) {
            if (Exists(path)) {
                var file = this.Client.Get(path);
                if (file.IsRegularFile) {
                    return new FilePath[] { };
                }
                return this.Client.ListDirectory(path)
                    .Where(e => e.Name != "." && e.Name != "..")
                    .Select(e => (FilePath)e.FullName);
            }
            return new FilePath[] { };
        }

        public virtual void CreateDirectory(FilePath path) {
            path = path.ToUnix();
            var files = path
                .ToString()
                .Split('/')
                .Where(e => !string.IsNullOrEmpty(e))
                .ToArray();
            var locator = new StringBuilder();
            for (var i = 0; i < files.Length; i++) {
                locator.Append("/");
                locator.Append(files[i]);
                var location = locator.ToString();
                if (!Exists(location)) {
                    this.Client.CreateDirectory(location);
                }
            }
        }

        public virtual Task WriteAsync(FilePath path, Stream stream) {
            path = path.ToUnix();
            CreateDirectory(path.Parent);
            return Task.Factory.FromAsync(
                this.Client.BeginUploadFile(stream, path),
                this.Client.EndUploadFile
                );
        }

        public async Task WriteAsync(FilePath path, byte[] content) {
            using (var stream = new MemoryStream(content)) {
                await WriteAsync(path, stream);
            }
        }

        public Task WriteAsync(FilePath path, string content) => WriteAsync(path, content, Encoding.UTF8);

        public Task WriteAsync(FilePath path, string content, Encoding encoding) {
            if (encoding == null) {
                throw new ArgumentNullException(nameof(encoding));
            }
            return WriteAsync(path, encoding.GetBytes(content));
        }

        public async Task<byte[]> ReadBytesAsync(FilePath path) {
            using (var input = OpenRead(path))
            using (var output = new MemoryStream()) {
                await input.CopyToAsync(output);
                return output.ToArray();
            }
        }

        public Task<string> ReadTextAsync(FilePath path) => ReadTextAsync(path, Encoding.UTF8);

        public async Task<string> ReadTextAsync(FilePath path, Encoding encoding) {
            if (encoding == null) {
                throw new ArgumentNullException(nameof(encoding));
            }
            var bytes = await ReadBytesAsync(path);
            return encoding.GetString(bytes);
        }

        public virtual Stream OpenRead(FilePath path) {
            return this.Client.OpenRead(path?.ToUnix());
        }
    }
}
