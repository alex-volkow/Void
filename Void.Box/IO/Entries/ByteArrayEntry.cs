using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.IO
{
    public class ByteArrayEntry : IEntryReader, IEntryWriter, IEntryAsyncWriter
    {
        private byte[] bytes;


        
        public FilePath Path { get; }

        public long Length => this.bytes.Length;



        public ByteArrayEntry(string path, IEnumerable<byte> bytes) {
            this.Path = path ?? throw new ArgumentNullException(nameof(path));
            this.bytes = bytes?.ToArray() ?? new byte[] { };
        }

        public ByteArrayEntry(string path)
            : this(path, null) {
        }



        public Stream Read() {
            return new MemoryStream(this.bytes);
        }

        public void Write(Stream stream) {
            this.bytes = stream.ToArray() ?? new byte[] { };
        }

        public async Task WriteAsync(Stream stream) {
            this.bytes = await stream?.ToArrayAsync() ?? new byte[] { };
        }
    }
}
