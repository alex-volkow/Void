using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Void.IO
{
    /// <summary>
    /// Represents universal filesystem path.
    /// </summary>
    public sealed class FilePath : IEquatable<FilePath>, IComparable<FilePath>, IEquatable<string>, IComparable<string>, ICloneable
    {
        private static readonly string WINDOWS_SEPARATOR = "\\";
        private static readonly string UNIX_SEPARATOR = "/";
        private static readonly IReadOnlyList<string> INVALID_CHARS = Path.GetInvalidFileNameChars()
            .Select(e => e.ToString())
            .Except(new string[] { "\\", "/", ":" })
            .ToArray();


        private readonly Lazy<FilePath> parent;
        private readonly Lazy<string> name;
        private readonly string path;


        /// <summary>
        /// Get parent file.
        /// </summary>
        public FilePath Parent => this.parent.Value;

        /// <summary>
        /// Get file name.
        /// </summary>
        public string Name => this.name.Value;

        /// <summary>
        /// Determine if the path is absolute.
        /// </summary>
        public bool IsAbsolute => !path.StartsWith(WINDOWS_SEPARATOR) && Path.IsPathRooted(this.path);
        
        /// <summary>
        /// Determine if the path has Unix format.
        /// </summary>
        public bool IsUnix => !this.path.Contains(WINDOWS_SEPARATOR);
        
        /// <summary>
        /// Determine if the path has Windows format.
        /// </summary>
        public bool IsWindows => !this.path.Contains(UNIX_SEPARATOR);

        private static bool IsWindowsSystem => Path.DirectorySeparatorChar == WINDOWS_SEPARATOR[0];


        /// <summary>
        /// Initialize a new instance by absolute or relative path.
        /// </summary>
        /// <exception cref="ArgumentException">Invalid path.</exception>
        /// <exception cref="ArgumentNullException">Path is null.</exception>
        /// <exception cref="System.Security.SecurityException">No required permissions.</exception>
        /// <exception cref="NotSupportedException">Path contains a colon (":").</exception>
        /// <exception cref="PathTooLongException">Path has too long length.</exception>
        public FilePath(string path) {
            Path.GetFullPath(path);
            foreach (var c in INVALID_CHARS) {
                if (path.Contains(c)) {
                    throw new ArgumentException(
                        "Invalid path"
                        );
                }
            }
            this.parent = new Lazy<FilePath>(GetParent);
            this.name = new Lazy<string>(GetName);
            this.path = path;
        }



        public static implicit operator FilePath(string path) => new FilePath(path);

        public static implicit operator string(FilePath path) => path?.ToString();

        public static FilePath operator +(FilePath left, FilePath right) => Combine(left, right);


        /// <summary>
        /// Combine multiple instance to one.
        /// </summary>
        public static FilePath Combine(params FilePath[] paths) {
            return Combine((IEnumerable<FilePath>)paths);
        }

        /// <summary>
        /// Combine multiple instance to one.
        /// </summary>
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

        /// <summary>
        /// Create a new instance normalized to current filesystem.
        /// </summary>
        /// <returns>New normalizaed instance.</returns>
        public FilePath Normalize() {
            return IsWindowsSystem ? ToWindows() : ToUnix();
        }

        /// <summary>
        /// Cast current path to Unix format.
        /// </summary>
        /// <returns>New instance in Unix format.</returns>
        public FilePath ToUnix() {
            if (this.IsUnix) {
                return this;
            }
            var path = this.path.Replace(WINDOWS_SEPARATOR, UNIX_SEPARATOR);
            if (this.IsAbsolute) {
                return new FilePath($"{UNIX_SEPARATOR}{path.Remove(":")}");
            }
            else if (path.StartsWith(UNIX_SEPARATOR)) {
                path = path.RemoveFirst(UNIX_SEPARATOR);
            }
            return new FilePath(path);
        }

        /// <summary>
        /// Cast current path to Windows format.
        /// </summary>
        /// <returns>New instance in Windows format.</returns>
        public FilePath ToWindows() {
            if (this.IsWindows) {
                return this;
            }
            var path = this.path.Replace(UNIX_SEPARATOR, WINDOWS_SEPARATOR);
            if (path.StartsWith(WINDOWS_SEPARATOR)) {
                path = path.RemoveFirst(WINDOWS_SEPARATOR);
                var volume = path.Split(WINDOWS_SEPARATOR[0])
                    .Where(e => !string.IsNullOrEmpty(e))
                    .FirstOrDefault();
                if (volume.Length == 1) {
                    path = path.ReplaceFirst(volume, $"{volume.ToUpper()}:");
                }
            }
            return new FilePath(path);
        }

        public bool Contains(FilePath path) {
            if (path == null) {
                throw new ArgumentNullException(nameof(path));
            }
            if (path.path.Length > this.path.Length) {
                return false;
            }
            var currentNames = this.Split();
            var otherNames = path.Split();
            if (otherNames.Length > currentNames.Length) {
                return false;
            }
            for (var i = 0; i < otherNames.Length; i++) {
                if (otherNames[i] != currentNames[i]) {
                    return false;
                }
            }
            return true;
        }

        public string[] Split() {
            return this.path.Split(
                new char[] { '\\', '/' }, 
                StringSplitOptions.RemoveEmptyEntries
                );
        }

        /// <inheritdoc/>
        public override string ToString() {
            return this.path;
        }

        /// <inheritdoc/>
        public object Clone() {
            return new FilePath(this.path);
        }

        /// <inheritdoc/>
        public int CompareTo(string other) {
            return this.path.CompareTo(other);
        }

        /// <inheritdoc/>
        public int CompareTo(FilePath other) {
            return CompareTo(other?.path);
        }

        /// <inheritdoc/>
        public bool Equals(string other) {
            if (other == null) {
                return false;
            }
            var thisParts = this.path
                .Split(UNIX_SEPARATOR[0], WINDOWS_SEPARATOR[0])
                .Where(e => !string.IsNullOrEmpty(e))
                .ToArray();
            var otherParts = other
                .Split(UNIX_SEPARATOR[0], WINDOWS_SEPARATOR[0])
                .Where(e => !string.IsNullOrEmpty(e))
                .ToArray();
            if (thisParts.Length != otherParts.Length) {
                return false;
            }
            for (var i = 0; i < thisParts.Length; i++) {
                if (i == 0 && thisParts[i]?.Length == 1 && otherParts[i]?.Length == 1) {
                    if (!string.Equals(thisParts[i], otherParts[i], StringComparison.OrdinalIgnoreCase)) {
                        return false;
                    }
                    else {
                        continue;
                    }
                }
                if (!string.Equals(thisParts[i], otherParts[i])) {
                    return false;
                }
            }
            return true;
        }

        /// <inheritdoc/>
        public bool Equals(FilePath other) {
            return Equals(other?.ToString());
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) {
            if (obj is string text) {
                return Equals(text);
            }
            if (obj is FilePath path) {
                return Equals(path);
            }
            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            return this.path.GetHashCode();
        }

        private FilePath GetParent() {
            var parts = (IEnumerable<string>)this.path
                    .TrimEnd(UNIX_SEPARATOR[0], WINDOWS_SEPARATOR[0])
                    .Replace(UNIX_SEPARATOR, $"\n{UNIX_SEPARATOR}\n")
                    .Replace(WINDOWS_SEPARATOR, $"\n{WINDOWS_SEPARATOR}\n")
                    .Split('\n');
            if (parts.Count() == 1) {
                return null;
            }
            parts = parts.Take(parts.Count() - 1);
            return new FilePath(string.Join(string.Empty, parts));
        }

        private string GetName() {
            return this.path
                .Trim(UNIX_SEPARATOR[0], WINDOWS_SEPARATOR[0])
                .Split(UNIX_SEPARATOR[0], WINDOWS_SEPARATOR[0])
                .Last();
        }
    }
}
