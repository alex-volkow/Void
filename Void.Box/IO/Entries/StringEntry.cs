using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.IO
{
    public class StringEntry : ByteArrayEntry
    {
        public StringEntry(string path) 
            : base(path) {
        }

        public StringEntry(string path, IEnumerable<byte> bytes) 
            : base(path, bytes) {
        }

        public StringEntry(string path, string value)
            : this(path, value, default(Encoding)) {
        }

        public StringEntry(string path, string value, Encoding encoding)
            : base(path, (encoding ?? Encoding.UTF8).GetBytes(value ?? string.Empty)) {
        }
    }
}
