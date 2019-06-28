using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Void.IO
{
    public static class StreamExtensions
    {
        public static async Task<byte[]> ToArrayAsync(this Stream stream) {
            using (var memory = new MemoryStream()) {
                if (stream.CanSeek) {
                    stream.Position = 0;
                }
                await stream.CopyToAsync(memory);
                return memory.ToArray();
            }
        }

        public static byte[] ToArray(this Stream stream) {
            using (var memory = new MemoryStream()) {
                if (stream.CanSeek) {
                    stream.Position = 0;
                }
                stream.CopyTo(memory);
                return memory.ToArray();
            }
        }

        public static string GetSHA256(this Stream stream) {
            using (var engine = SHA256Managed.Create()) {
                return Convert.ToBase64String(engine.ComputeHash(stream));
            }
        }
    }
}
