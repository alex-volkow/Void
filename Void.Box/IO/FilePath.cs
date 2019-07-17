﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Void.IO
{
    public sealed class FilePath : IEquatable<FilePath>, IComparable<FilePath>, IEquatable<string>, IComparable<string>, ICloneable
    {
        private static readonly string WINDOWS_SEPARATOR = "\\";
        private static readonly string UNIX_SEPARATOR = "/";
        private static readonly IReadOnlyList<string> INVALID_CHARS = Path.GetInvalidFileNameChars()
            .Select(e => e.ToString())
            .Except(new string[] { "\\", "/", ":" })
            .ToArray();


        private readonly string path;



        public FilePath Parent {
            get {
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
        }

        public string Name {
            get {
                return this.path
                    .Trim(UNIX_SEPARATOR[0], WINDOWS_SEPARATOR[0])
                    .Split(UNIX_SEPARATOR[0], WINDOWS_SEPARATOR[0])
                    .Last();
            }
        }

        public bool IsAbsolute => !path.StartsWith(WINDOWS_SEPARATOR) && Path.IsPathRooted(this.path);

        public bool IsUnix => !this.path.Contains(WINDOWS_SEPARATOR);

        public bool IsWindows => !this.path.Contains(UNIX_SEPARATOR);

        private static bool IsWindowsSystem => Path.DirectorySeparatorChar == WINDOWS_SEPARATOR[0];



        public FilePath(string path) {
            Path.GetFullPath(path);
            foreach (var c in INVALID_CHARS) {
                if (path.Contains(c)) {
                    throw new ArgumentException(
                        "Invalid path"
                        );
                }
            }
            this.path = path;
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
                return new FilePath($"{UNIX_SEPARATOR}{path.Remove(":").ToLower()}");
            }
            else if (path.StartsWith(UNIX_SEPARATOR)) {
                path = path.RemoveFirst(UNIX_SEPARATOR);
            }
            return new FilePath(path);
        }

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
            if (other == null) {
                return false;
            }
            var thisParts = this.path
                .Split(UNIX_SEPARATOR[0], WINDOWS_SEPARATOR[0])
                .Where(e => !string.IsNullOrEmpty(e));
            var otherParts = other
                .Split(UNIX_SEPARATOR[0], WINDOWS_SEPARATOR[0])
                .Where(e => !string.IsNullOrEmpty(e));
            return thisParts.Count() == otherParts.Count() && thisParts.SequenceEqual(otherParts);
        }

        public bool Equals(FilePath other) {
            return Equals(other?.ToString());
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
