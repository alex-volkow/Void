using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Void.IO
{
    public static class Files
    {
        private static readonly Lazy<FileInfo> entryPoint = new Lazy<FileInfo>(() => new FileInfo(Assembly
            .GetEntryAssembly().Location
            ));


        public static FileInfo EntryPoint {
            get {
                return entryPoint.Value;
            }
        }


        public static bool PathEqual(string path1, string path2) {
            if (string.IsNullOrWhiteSpace(path1) || 
                string.IsNullOrWhiteSpace(path2) || 
                !IsValid(path1) && 
                !IsValid(path2)) {
                return false;
            }
            path1 = path1.Replace("\\", "/");
            path2 = path2.Replace("\\", "/");
            if (IsRelative(path1) && IsRelative(path2)) {
                if (path1.StartsWith("/")) {
                    path1 = path1.RemoveFirst("/");
                }
                if (path2.StartsWith("/")) {
                    path2 = path2.RemoveFirst("/");
                }
            }
            return path1 == path2;
        }

        public static string AsAbsolute(string path) {
            return Files.IsRelative(path)
                ? Files.EntryPoint.Directory.Combine(path)
                : path;
        }

        public static bool IsRelative(string path) {
            Uri uri;
            try {
                uri = new Uri(path, UriKind.RelativeOrAbsolute);
            }
            catch {
                throw new InvalidOperationException(string.Format(
                    "Invalid path value: {0}", path
                    ));
            }
            return !uri.IsAbsoluteUri || (uri.IsFile && uri.IsUnc);
        }

        public static string CombineRelative(params string[] pathes) {
            var components = new List<string>(pathes.Length + 1) {
                EntryPoint.DirectoryName
            };
            components.AddRange(pathes);
            return Combine(components.ToArray());
        }

        public static string GetTempPath(params string[] pathes) {
            return Path.Combine(Path.GetTempPath(), Path.Combine(pathes));
        }

        public static DirectoryInfo CreateTempDirectory(params string[] pathes) {
            var path = GetTempPath(pathes);
            var directory = new DirectoryInfo(path);
            if (!directory.Exists) {
                directory.Create();
            }
            return directory;
        }

        public static FileInfo CreateTempFile(params string[] pathes) {
            var path = pathes.Length == 0
                ? Path.GetTempFileName()
                : GetTempPath(pathes);
            var file = new FileInfo(path);
            if (!file.Directory.Exists) {
                file.Directory.Create();
            }
            if (!file.Exists) {
                file.Create().Close();
            }
            return file;
        }

        public static bool IsValid(string path) {
            if (!string.IsNullOrEmpty(path)) {
                path = Regex.Replace(path, "(^([a-zA-Z]:)?[\\\\/]?)|([\\\\/]$)", string.Empty);
                foreach (var name in Regex.Split(path, "[\\\\/]")) {
                    if (string.IsNullOrEmpty(name)) {
                        return false;
                    }
                    foreach (var value in Path.GetInvalidFileNameChars()) {
                        if (name.Contains(value)) {
                            return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public static string Combine(string path) {
            return Combine(new string[] { path });
        }

        public static string Combine(string path1, string path2) {
            return Combine(new string[] { path1, path2 });
        }

        public static string Combine(params string[] pathes) {
            var values = new string[pathes.Length];
            for (var i = 0; i < pathes.Length; i++) {
                values[i] = pathes[i];
                values[i] = pathes[i].StartsWith(@"\") ? pathes[i].RemoveFirst(@"\") : values[i];
                values[i] = pathes[i].StartsWith(@"/") ? pathes[i].RemoveFirst(@"/") : values[i];
            }
            return Path.Combine(values);
        }

        public static bool IsLocked(this FileInfo file) {
            var stream = default(FileStream);
            try {
                stream = file.Open(
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.None
                    );
            }
            catch (IOException) {
                return true;
            }
            finally {
                if (stream != null) {
                    stream.Close();
                }
            }
            return false;
        }

        public static bool IsDirectory(string path) {
            return (File.Exists(path) || Directory.Exists(path))
                && File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }

        public static bool IsFile(string path) {
            return (File.Exists(path) || Directory.Exists(path))
                && !File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }
    }
}
