using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.IO
{
    public static class ZipEntries
    {
        public static FileInfo Save(IEnumerable<IEntryReader> entries, string path) {
            var file = new FileInfo(path);
            file.InitializeDirectory();
            using (var stream = File.OpenWrite(path)) {
                WriteTo(entries, stream);
                return file;
            }
        }

        public static async Task<FileInfo> SaveAsync(IEnumerable<IEntryReader> entries, string path) {
            var file = new FileInfo(path);
            file.InitializeDirectory();
            using (var stream = File.OpenWrite(path)) {
                await WriteToAsync(entries, stream);
                return file;
            }
        }

        public static void WriteTo(IEnumerable<IEntryReader> entries, Stream memory) {
            using (var archive = new ZipArchive(memory, ZipArchiveMode.Create, true)) {
                foreach (var item in entries) {
                    var entry = archive.CreateEntry(item.Path, CompressionLevel.Optimal);
                    using (var output = entry.Open())
                    using (var input = item.Read()) {
                        input.CopyTo(output);
                    }
                }
            }
        }

        public static async Task WriteToAsync(IEnumerable<IEntryReader> entries, Stream memory) {
            using (var archive = new ZipArchive(memory, ZipArchiveMode.Create, true)) {
                foreach (var item in entries) {
                    var entry = archive.CreateEntry(item.Path, CompressionLevel.Optimal);
                    using (var output = entry.Open())
                    using (var input = item.Read()) {
                        await input.CopyToAsync(output);
                    }
                }
            }
        }

        public static byte[] ToArray(IEnumerable<IEntryReader> entries) {
            using (var memory = new MemoryStream()) {
                WriteTo(entries, memory);
                return memory.ToArray();
            }
        }

        public static async Task<byte[]> ToArrayAsync(IEnumerable<IEntryReader> entries) {
            using (var memory = new MemoryStream()) {
                await WriteToAsync(entries, memory);
                return await memory.ToArrayAsync();
            }
        }

        public static IEnumerable<IEntryReader> Import(Stream stream) {
            var items = new List<ByteArrayEntry>();
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, true)) {
                foreach (var entry in archive.Entries) {
                    using (var source = entry.Open()) {
                        items.Add(new ByteArrayEntry(
                            entry.FullName, 
                            source.ToArray()
                            ));
                    }
                }
            }
            return items;
        }

        public static async Task<IEnumerable<IEntryReader>> ImportAsync(Stream stream) {
            var items = new List<ByteArrayEntry>();
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Read, true)) {
                foreach (var entry in archive.Entries) {
                    using (var source = entry.Open()) {
                        var content = await source.ToArrayAsync();
                        items.Add(new ByteArrayEntry(
                            entry.FullName,
                            content
                            ));
                    }
                }
            }
            return items;
        }

        public static async Task<IEnumerable<IEntryReader>> ImportAsync(FileInfo file) {
            using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read)) {
                return await ImportAsync(stream);
            }
        }
    }
}
