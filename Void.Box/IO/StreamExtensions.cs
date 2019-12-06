﻿using System;
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
            return stream.GetHash(MD5.Create());
        }

        public static string GetSHA256(this Stream stream) {
            return stream.GetHash(SHA256Managed.Create());
        }

        public static string GetSHA512(this Stream stream) {
            return stream.GetHash(SHA512Managed.Create());
        }

        public static Task<string> GetMD5(this Stream stream, CancellationToken token = default) {
            return stream.GetHashAsync(MD5.Create(), token);
        }

        public static Task<string> GetSHA256(this Stream stream, CancellationToken token = default) {
            return stream.GetHashAsync(SHA256Managed.Create(), token);
        }

        public static Task<string> GetSHA512(this Stream stream, CancellationToken token = default) {
            return stream.GetHashAsync(SHA512Managed.Create(), token);
        }

        private static string GetHash(this Stream stream, HashAlgorithm engine) {
            using (engine) {
                var hash = engine.ComputeHash(stream);
                var builder = new StringBuilder();
                foreach (var value in hash) {
                    builder.Append(value.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private static async Task<string> GetHashAsync(this Stream stream, HashAlgorithm engine, CancellationToken token) {
            using (engine) {
                var bytes = await stream.ReadToEndAsync(token);
                var hash = engine.ComputeHash(stream);
                var builder = new StringBuilder();
                foreach (var value in hash) {
                    builder.Append(value.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
