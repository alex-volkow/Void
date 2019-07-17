using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Void.Net
{
    public sealed class FilePath : IEquatable<FilePath>, IComparable<FilePath>, IEquatable<string>, IComparable<string>, ICloneable
    {
        private static readonly string WINDOWS_SEPARATOR = "\\";
        private static readonly string UNIX_SEPARATOR = "/";

        private readonly string path;



        public FilePath Parent => new FilePath(Path.GetDirectoryName(this.path));

        public string Name => Path.GetFileName(this.path);

        public bool IsAbsolute => Path.IsPathRooted(this.path);

        public bool IsUnix => this.path.Contains(UNIX_SEPARATOR);

        public bool IsWindows => this.path.Contains(WINDOWS_SEPARATOR);

        private static bool IsWindowsSystem => Path.DirectorySeparatorChar == WINDOWS_SEPARATOR[0];



        public FilePath(string path) {
            this.path = path?.Trim() ?? throw new ArgumentNullException(nameof(path));
            if (!Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute)) {
                throw new FormatException("Invalid path format");
            }
        }



        public static implicit operator FilePath(string path) => new FilePath(path);

        public static implicit operator string(FilePath path) => path?.ToString();

        public static FilePath operator +(FilePath left, FilePath right) => Combine(left, right);

        public static FilePath Combine(params FilePath[] paths) {
            return Combine((IEnumerable<FilePath>)paths);
        }

        public static FilePath Combine(IEnumerable<FilePath> paths) {
            if (paths == null) {
                throw new ArgumentNullException(nameof(paths));
            }
            paths = paths.Where(e => e != null);
            if (!paths.Any()) {
                return IsWindowsSystem
                    ? new FilePath(WINDOWS_SEPARATOR)
                    : new FilePath(UNIX_SEPARATOR);
            }
            if (paths.All(e => e.IsWindows)) {
                return new FilePath(string.Join(WINDOWS_SEPARATOR,
                    paths.Select(e => e.path.Trim(WINDOWS_SEPARATOR[0]))
                    ));
            }
            if (paths.All(e => e.IsUnix)) {
                return new FilePath(
                    string.Join(UNIX_SEPARATOR, paths.Select(e => e.path))
                    .Replace($"{UNIX_SEPARATOR}{UNIX_SEPARATOR}", UNIX_SEPARATOR)
                    );
            }
            return IsWindowsSystem
                ? Combine(paths.Select(e => e.ToWindows()))
                : Combine(paths.Select(e => e.ToUnix()));
        }

        public FilePath Normalize() {
            return IsWindowsSystem ? ToWindows() : ToUnix();
        }

        public FilePath ToUnix() {
            if (this.IsUnix) {
                return this;
            }
            var path = this.path.Replace(WINDOWS_SEPARATOR, UNIX_SEPARATOR);
            if (this.IsAbsolute) {
                return new FilePath($"{UNIX_SEPARATOR}{path.Remove(":")}");
            }
            if (path.StartsWith(UNIX_SEPARATOR)) {
                path.RemoveFirst(UNIX_SEPARATOR);
            }
            return new FilePath(path);
        }

        public FilePath ToWindows() {
            if (this.IsWindows) {
                return this;
            }
            return new FilePath(this.path
                .Replace(UNIX_SEPARATOR, WINDOWS_SEPARATOR)
                .RemoveFirst(WINDOWS_SEPARATOR)
                );
        }

        public override string ToString() {
            return this.path;
        }

        public object Clone() {
            return new FilePath(this.path);
        }

        public int CompareTo(string other) {
            return this.path.CompareTo(other);
        }

        public int CompareTo(FilePath other) {
            return CompareTo(other?.path);
        }

        public bool Equals(string other) {
            return ToWindows().Equals(other);
        }

        public bool Equals(FilePath other) {
            return Equals(other?.ToWindows());
        }

        public override bool Equals(object obj) {
            if (obj is string text) {
                return Equals(text);
            }
            if (obj is FilePath path) {
                return Equals(path);
            }
            return false;
        }

        public override int GetHashCode() {
            return this.path.GetHashCode();
        }
    }
}
