using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.IO
{
    public static class EntryExtensions
    {
        public static async Task<string> ReadStringAsync(this IEntryReader entry, Encoding encoding) {
            using (var stream  = entry.Read()) {
                var bytes = await stream.ToArrayAsync();
                return encoding.GetString(bytes);
            }
        }

        public static string GetSHA256(this IEntryReader entry) {
            using (var stream = entry.Read()) {
                return stream.GetSHA256();
            }
        }

        public static async Task<IEntryPack<FileEntry>> CopyToAsync(this IEntryPack<IEntryReader> entries, DirectoryInfo destination) {
            var pack = new EntryMap<FileEntry>();
            foreach (var entry in entries) {
                var file = new FileInfo(destination.Combine(entry.Value.Path));
                file.InitializeDirectory();
                using (var source = entry.Value.Read())
                using (var target = file.OpenWrite()) {
                    await source.CopyToAsync(target);
                    var path = file.GetRelativeName(destination);
                    pack.Add(new FileEntry(path, file));
                }
            }
            return pack;
        }
    }
}
