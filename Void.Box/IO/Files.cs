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
        //private static readonly Lazy<FileInfo> application = new Lazy<FileInfo>(() => new FileInfo(Process
        //               .GetCurrentProcess()
        //               .MainModule
        //               .FileName
        //               ));

        private static readonly Lazy<FileInfo> application = new Lazy<FileInfo>(() => new FileInfo(Assembly
            .GetEntryAssembly().Location
            ));


        public static FileInfo Application {
            get {
                return application.Value;
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

        public static byte[] ReadBytes(string path) {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                return stream.ToArray();
            }
        }

        public static string ReadText(string path, Encoding encoding) {
            return encoding.GetString(ReadBytes(path));
        }

        public static string ReadText(string path) {
            return ReadText(path, Encoding.UTF8);
        }

        public static async Task<byte[]> ReadBytesAsync(string path) {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                return await stream.ToArrayAsync();
            }
        }

        public static async Task<string> ReadTextAsync(string path, Encoding encoding) {
            var bytes = await ReadBytesAsync(path);
            return encoding.GetString(bytes);
        }

        public static Task<string> ReadTextAsync(string path) {
            return ReadTextAsync(path, Encoding.UTF8);
        }

        public static async Task WriteBytesAsync(string path, byte[] bytes) {
            using (var stream = File.Open(path, FileMode.Create, FileAccess.Write)) {
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        public static async Task AppendBytesAsync(string path, byte[] bytes) {
            using (var stream = File.Open(path, FileMode.Append, FileAccess.Write)) {
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        public static Task WriteTextAsync(string path, string text) {
            return WriteTextAsync(path, text, Encoding.UTF8);
        }

        public static Task WriteTextAsync(string path, string text, Encoding encoding) {
            return WriteBytesAsync(path, encoding.GetBytes(text));
        }

        public static Task AppendText(string path, string text) {
            return AppendText(path, text, Encoding.UTF8);
        }

        public static Task AppendText(string path, string text, Encoding encoding) {
            return AppendBytesAsync(path, encoding.GetBytes(text));
        }

        public static string AsAbsolute(string path) {
            return Files.IsRelative(path)
                ? Files.Application.Directory.Combine(path)
                : path;
        }

        public static bool IsRelative(string path) {
            var uri = default(Uri);
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
                Application.DirectoryName
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

        public static async Task CopyFileAsync(string from, string to) {
            using (var source = File.Open(from, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var target = File.Create(to)) {
                await source.CopyToAsync(target);
            }
        }

        public static void CopyTo(this Stream input, Stream output) {
            input.CopyTo(output, 8 * 1024);
        }

        public static void CopyTo(this Stream input, Stream output, int bufferSize) {
            var buffer = new byte[bufferSize];
            var length = default(int);
            while ((length = input.Read(buffer, 0, buffer.Length)) > 0) {
                output.Write(buffer, 0, length);
            }
        }

        public static bool IsDirectory(string path) {
            return (File.Exists(path) || Directory.Exists(path))
                && File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }

        public static bool IsFile(string path) {
            return (File.Exists(path) || Directory.Exists(path))
                && !File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }

        public static void AppendAllBytes(string path, byte[] bytes) {
            using (var stream = new FileStream(path, FileMode.Append)) {
                stream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
