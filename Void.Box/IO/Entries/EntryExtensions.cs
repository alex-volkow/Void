using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Void.IO
{
    public static class EntryExtensions
    {
        public static async Task<string> ReadStringAsync(this IEntryReader entry, Encoding encoding) {
            using (var stream  = entry.Open()) {
                var bytes = await stream.ToArrayAsync();
                return encoding.GetString(bytes);
            }
        }

        public static string GetSHA256(this IEntryReader entry) {
            using (var stream = entry.Open()) {
                return stream.GetSHA256();
            }
        }

        public static async Task<string> GetSHA256Async(this IEntryReader entry) {
            using (var stream = entry.Open()) {
                return await stream.GetSHA256Async();
            }
        }

        public static string GetSHA512(this IEntryReader entry) {
            using (var stream = entry.Open()) {
                return stream.GetSHA512();
            }
        }

        public static async Task<string> GetSHA512Async(this IEntryReader entry) {
            using (var stream = entry.Open()) {
                return await stream.GetSHA512Async();
            }
        }

        public static async Task<IEntryPack<FileEntry>> CopyToAsync(
            this IEntryPack<IEntryReader> entries, 
            DirectoryInfo destination, 
            CancellationToken token = default
            ) {
            var pack = new EntryMap<FileEntry>();
            foreach (var entry in entries) {
                var file = new FileInfo(destination.Combine(entry.Value.Path));
                file.InitializeDirectory();
                using (var source = entry.Value.Open())
                using (var target = file.OpenWrite()) {
                    await source.CopyToAsync(target, 81920, token);
                    var path = file.GetRelativeName(destination);
                    pack.Add(new FileEntry(path, file));
                }
            }
            return pack;
        }
    }
}
