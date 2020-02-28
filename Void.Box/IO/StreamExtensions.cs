using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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

        public static byte[] ReadToEnd(this Stream stream, int bufferSize = 1024) {
            var buffer = new byte[bufferSize];
            using (var memory = new MemoryStream()) {
                var count = 0;
                while ((count = stream.Read(buffer, 0, buffer.Length)) > 0) {
                    memory.Write(buffer, 0, count);
                }
                return memory.ToArray();
            }
        }

        public static Task<byte[]> ReadToEndAsync(
            this Stream stream,
            CancellationToken token = default
            ) {
            return stream.ReadToEndAsync(1024, token);
        }

        public static async Task<byte[]> ReadToEndAsync(
            this Stream stream, 
            int bufferSize = 1024, 
            CancellationToken token = default
            ) {
            var buffer = new byte[bufferSize];
            using (var memory = new MemoryStream()) {
                var count = 0;
                while ((count = await stream.ReadAsync(buffer, 0, buffer.Length, token)) > 0) {
                    await memory.WriteAsync(buffer, 0, count, token);
                }
                return memory.ToArray();
            }
        }

        public static string GetMD5(this Stream stream) {
            return stream.GetHashString(MD5.Create());
        }

        public static string GetSHA256(this Stream stream) {
            return stream.GetHashString(SHA256Managed.Create());
        }

        public static string GetSHA512(this Stream stream) {
            return stream.GetHashString(SHA512Managed.Create());
        }

        public static Task<string> GetMD5Async(this Stream stream, CancellationToken token = default) {
            return stream.GetHashStringAsync(MD5.Create(), token: token);
        }

        public static Task<string> GetSHA256Async(this Stream stream, CancellationToken token = default) {
            return stream.GetHashStringAsync(SHA256Managed.Create(), token: token);
        }

        public static Task<string> GetSHA512Async(this Stream stream, CancellationToken token = default) {
            return stream.GetHashStringAsync(SHA512Managed.Create(), token: token);
        }

        public static string GetHashString(this Stream stream, HashAlgorithm algorithm, string format = "x2") {
            using (algorithm) {
                var hash = algorithm.ComputeHash(stream);
                var builder = new StringBuilder();
                foreach (var value in hash) {
                    builder.Append(value.ToString(format));
                }
                return builder.ToString();
            }
        }

        public static async Task<string> GetHashStringAsync(this Stream stream, HashAlgorithm algorithm, string format = "x2", CancellationToken token = default) {
            using (algorithm ?? throw new ArgumentNullException(nameof(algorithm))) {
                var bytes = await stream.ReadToEndAsync(token);
                var hash = algorithm.ComputeHash(bytes);
                var builder = new StringBuilder();
                foreach (var value in hash) {
                    builder.Append(value.ToString(format));
                }
                return builder.ToString();
            }
        }
    }
}
