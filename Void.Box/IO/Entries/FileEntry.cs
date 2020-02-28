using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.IO
{
    public class FileEntry : IEntryReader, IEntryWriter, IEntryAsyncWriter
    {
        public FileInfo File { get; }


        public FilePath Path { get; }

        public long Length => this.File.Length;



        public FileEntry(string path, FileInfo file) {
            this.Path = path ?? throw new ArgumentNullException(nameof(path));
            this.File = file ?? throw new ArgumentNullException(nameof(file));
        }

        public FileEntry(DirectoryInfo location, FileInfo file)
            : this(file.GetRelativeName(location), file) {
        }



        public Stream Open() {
            return this.File.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public void Write(Stream stream) {
            using (var file = this.File.OpenWrite()) {
                stream.CopyTo(file);
            }
        }

        public async Task WriteAsync(Stream stream) {
            using (var file = this.File.OpenWrite()) {
                await stream.CopyToAsync(file);
            }
        }
    }
}
