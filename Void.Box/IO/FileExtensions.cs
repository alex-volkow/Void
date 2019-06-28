using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Void.IO
{
    public static class FileExtensions
    {
        public static string GetRelativeName(this FileSystemInfo file, DirectoryInfo directory) {
            var name = file?.FullName?.Remove(directory?.FullName ?? string.Empty);
            if (name?.StartsWith("\\") ?? false) {
                name = name.RemoveFirst("\\");
            }
            return name;
        }

        //public static string GetRelativeName(this FileSystemInfo file) {
        //    if (Directory.GetCurrentDirectory() == Files.Application.DirectoryName) {
        //        return file?.FullName?.Replace(
        //            Files.Application.DirectoryName,
        //            string.Empty
        //            );
        //    }
        //    return file?.FullName;
        //}

        public static string GetMD5(this FileInfo file) {
            using (var md5 = MD5.Create()) {
                using (var stream = file.OpenRead()) {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter
                        .ToString(hash)
                        .Replace("-", string.Empty)
                        .ToLowerInvariant();
                }
            }
        }

        public static string GetSHA256(this FileInfo file) {
            using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read)) {
                return stream.GetSHA256();
            }
        }

        public static IEnumerable<FileInfo> FindFiles(this DirectoryInfo directory, string regex) {
            return directory.FindFiles(new Regex(regex));
        }

        public static IEnumerable<FileInfo> FindFiles(this DirectoryInfo directory, Regex regex) {
            foreach (var file in directory.GetContent()) {
                if (regex.IsMatch(file.FullName)) {
                    yield return file;
                }
            }
        }

        public static void InitializeDirectory(this FileInfo file) {
            Initialize(file.Directory);
        }

        public static void Initialize(this DirectoryInfo directory) {
            if (!directory.Exists) {
                directory.Create();
            }
        }

        public static string Combine(this FileSystemInfo path, params string[] pathes) {
            var components = new List<string>(pathes.Length + 1) { path.FullName };
            components.AddRange(pathes);
            return Files.Combine(components.ToArray());
        }

        public static IEnumerable<FileInfo> EnumerateContent(this DirectoryInfo source) {
            if (source?.Exists ?? false) {
                foreach (var file in source.EnumerateFiles()) {
                    yield return file;
                }
                foreach (var folder in source.EnumerateDirectories()) {
                    foreach (var file in folder.EnumerateFiles()) {
                        yield return file;
                    }
                }
            }
        }

        //public static IEnumerable<FileInfo> EnumerateAccessibleContent(this DirectoryInfo source) {
        //    if (source?.Exists ?? false) {
        //        foreach (var file in source.TryEnumerateFiles()) {
        //            yield return file;
        //        }
        //        foreach (var directory in source.TryEnumerateDirectories()) {
        //            foreach (var file in directory.EnumerateAccessibleContent()) {
        //                yield return file;
        //            }
        //        }
        //    }
        //}

        public static IEnumerable<FileInfo> GetAccessibleContent(this DirectoryInfo source) {
            if (source?.Exists ?? false) {
                foreach (var file in source.TryGetFiles()) {
                    yield return file;
                }
                foreach (var directory in source.TryGetDirectories()) {
                    foreach (var file in directory.GetAccessibleContent()) {
                        yield return file;
                    }
                }
            }
        }

        public static IReadOnlyList<FileInfo> GetContent(this DirectoryInfo source, string pattern) {
            var content = GetContent(source);
            if (pattern != null) {
                return content
                    .Where(item => Regex.IsMatch(item.FullName, pattern))
                    .ToArray();
            }
            return content;
        }

        public static IReadOnlyList<FileInfo> GetAvailableContent(this DirectoryInfo source) {
            var files = new List<FileInfo>();
            try { files.AddRange(source.GetFiles()); } catch { }
            try {
                foreach (var directory in source.GetDirectories()) {
                    files.AddRange(directory.GetAvailableContent());
                }
            }
            catch { }
            return files;
        }

        public static IReadOnlyList<FileInfo> GetContent(this DirectoryInfo source) {
            var files = new List<FileInfo>(source.GetFiles());
            foreach (var directory in source.GetDirectories()) {
                files.AddRange(directory.GetContent());
            }
            return files;
        }

        public static bool TryDeleteWithContent(this DirectoryInfo directory) {
            if (directory != null && Directory.Exists(directory.FullName)) {
                foreach (var file in directory.GetFiles()) {
                    try { file.Delete(); } catch { }
                }
                foreach (var folder in directory.GetDirectories()) {
                    folder.DeleteWithContent();
                }
                try { directory.Delete(); } catch { }
                return true;
            }
            return false;
        }

        public static bool DeleteWithContent(this DirectoryInfo directory) {
            if (directory != null && Directory.Exists(directory.FullName)) {
                foreach (var file in directory.GetFiles()) {
                    file.Delete();
                }
                foreach (var folder in directory.GetDirectories()) {
                    folder.DeleteWithContent();
                }
                directory.Delete();
                return true;
            }
            return false;
        }

        public static async Task CopyToAsync(this FileInfo file, string path) {
            using (var source = File.Open(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var target = File.Create(path)) {
                await source.CopyToAsync(target);
            }
        }

        public static Task<IReadOnlyList<FileInfo>> CopyContentAsync(this DirectoryInfo source, DirectoryInfo destination) {
            return source.CopyContentAsync(destination, null);
        }

        public static async Task<IReadOnlyList<FileInfo>> CopyContentAsync(
            this DirectoryInfo source,
            DirectoryInfo destination,
            Func<FileSystemInfo, bool> condition
            ) {
            //destination.Initialize();
            var files = new List<FileInfo>();
            foreach (var file in source.GetFiles()) {
                if (condition == null || condition(file)) {
                    var clone = new FileInfo(Path.Combine(destination.FullName, file.Name));
                    clone.InitializeDirectory();
                    //if (clone.Exists) {
                    //    clone.Delete();
                    //}
                    await file.CopyToAsync(clone.FullName);
                    files.Add(clone);
                }
            }
            foreach (var directory in source.GetDirectories()) {
                if (condition == null || condition(directory)) {
                    var clone = new DirectoryInfo(Path.Combine(
                        destination.FullName,
                        directory.Name
                        ));
                    var copies = await directory.CopyContentAsync(clone, condition);
                    files.AddRange(copies);
                }
            }
            return files;
        }

        public static IReadOnlyList<FileInfo> CopyContent(this DirectoryInfo source, DirectoryInfo destination) {
            return source.CopyContent(destination, null);
        }

        public static IReadOnlyList<FileInfo> CopyContent(
            this DirectoryInfo source,
            DirectoryInfo destination,
            Func<FileSystemInfo, bool> condition
            ) {
            //destination.Initialize();
            var files = new List<FileInfo>();
            foreach (var file in source.GetFiles()) {
                if (condition == null || condition(file)) {
                    var clone = new FileInfo(Path.Combine(destination.FullName, file.Name));
                    clone.InitializeDirectory();
                    //if (clone.Exists) {
                    //    clone.Delete();
                    //}
                    file.CopyTo(clone.FullName, true);
                    files.Add(clone);
                }
            }
            foreach (var directory in source.GetDirectories()) {
                if (condition == null || condition(directory)) {
                    var clone = new DirectoryInfo(Path.Combine(
                        destination.FullName,
                        directory.Name
                        ));
                    files.AddRange(directory.CopyContent(
                        clone,
                        condition
                        ));
                }
            }
            return files;
        }

        private static IEnumerable<FileInfo> TryGetFiles(this DirectoryInfo directory) {
            try {
                return directory.GetFiles();
            }
            catch {
                return new FileInfo[] { };
            }
        }

        private static IEnumerable<DirectoryInfo> TryGetDirectories(this DirectoryInfo directory) {
            try {
                return directory.GetDirectories();
            }
            catch {
                return new DirectoryInfo[] { };
            }
        }
    }
}
