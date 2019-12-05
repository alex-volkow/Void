using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Void.IO
{
    [Parallelizable]
    public class FilePathTests
    {
        private static readonly IReadOnlyList<string> VALID_PATHS = new string[] {
            "C:\\directory\\file.txt",
            "\\directory\\file.txt",
            "directory\\file.txt",
            "\\file.txt",
            "file.txt",
            "/var/directory/file.txt",
            "directory/file.txt",
            "file.txt"
        };

        private static readonly IReadOnlyList<string> INVALID_PATHS = new string[] {
            "C:\\directory?\\file.txt",
            "\\direc\r\ntory\\file.txt",
            "!@#$%^&*():",
            null
        };

        private static readonly IReadOnlyList<string> ABSOLUTE_PATHS = new string[] {
            "C:\\directory\\file.txt",
            "/var/directory/file.txt"
        };

        private static readonly IReadOnlyList<string> RELATIVE_PATHS = new string[] {
            "\\directory\\file.txt",
            "directory\\file.txt",
            "\\file.txt",
            "file.txt",
            "directory/file.txt",
            "file.txt"
        };

        private static readonly IReadOnlyList<string> WINDOWS_PATHS = new string[] {
            "C:\\directory\\file.txt",
            "\\directory\\file.txt",
            "directory\\file.txt",
            "\\file.txt",
            "file.txt"
        };

        private static readonly IReadOnlyList<string> WINDOWS_TO_UNIX_PATHS = new string[] {
            "/c/directory/file.txt",
            "directory/file.txt",
            "directory/file.txt",
            "file.txt",
            "file.txt"
        };

        private static readonly IReadOnlyList<string> UNIX_PATHS = new string[] {
            "/var/directory/file.txt",
            "directory/file.txt",
            "file.txt",
            "/c/file"
        };

        private static readonly IReadOnlyList<string> UNIX_TO_WINDOWS_PATHS = new string[] {
            "var\\directory\\file.txt",
            "directory\\file.txt",
            "file.txt",
            "C:\\file"
        };

        private static readonly IReadOnlyDictionary<string, string> NAMES = new Dictionary<string, string> {
            ["C:\\directory1\\file.txt"] = "file.txt",
            ["C:\\directory1\\"] = "directory1",
            ["C:\\directory1"] = "directory1",
            ["C:\\"] = "C:",
            ["C:"] = "C:",
            ["\\file.txt"] = "file.txt",
            ["file.txt"] = "file.txt",
            ["/var/directory2/file.txt"] = "file.txt",
            ["/var/directory2/"] = "directory2",
            ["/var/directory2"] = "directory2",
        };

        private static readonly IReadOnlyDictionary<string, string> PARENTS = new Dictionary<string, string> {
            ["C:\\directory1\\file.txt"] = "directory1",
            ["C:\\directory1\\"] = "C:",
            ["C:\\directory1"] = "C:",
            ["C:\\"] = null,
            ["C:"] = null,
            ["/var/directory2/file.txt"] = "directory2",
            ["/var/directory2/"] = "var",
            ["/var/directory2"] = "var",
            ["/var/"] = null,
            ["/var"] = null,
            ["/"] = null,
        };



        [Test]
        public void CreateSuccess() {
            foreach (var path in VALID_PATHS) {
                new FilePath(path);
            }
        }

        [Test]
        public void CreateInvalidFormat() {
            foreach (var path in INVALID_PATHS) {
                Assert.Throws<Exception>(() => new FilePath(path));
            }
        }

        [Test]
        public void CheckAbsolutePaths() {
            foreach (var path in ABSOLUTE_PATHS) {
                Assert.True(new FilePath(path).IsAbsolute, path);
            }
        }

        [Test]
        public void CheckNotAbsolutePaths() {
            foreach (var path in RELATIVE_PATHS) {
                Assert.False(new FilePath(path).IsAbsolute, path);
            }
        }

        [Test]
        public void CheckRelativePaths() {
            foreach (var path in RELATIVE_PATHS) {
                Assert.False(new FilePath(path).IsAbsolute, path);
            }
        }

        [Test]
        public void CheckUnixPaths() {
            foreach (var path in UNIX_PATHS) {
                Assert.True(new FilePath(path).IsUnix, path);
            }
        }

        [Test]
        public void CheckWindowsPaths() {
            foreach (var path in WINDOWS_PATHS) {
                Assert.True(new FilePath(path).IsWindows, path);
            }
        }

        [Test]
        public void CheckNotUnixPaths() {
            foreach (var path in WINDOWS_PATHS.Where(e => e.Contains("\\") || e.Contains("/"))) {
                Assert.False(new FilePath(path).IsUnix, path);
            }
        }

        [Test]
        public void CheckNotWindowsPaths() {
            foreach (var path in UNIX_PATHS.Where(e => e.Contains("\\") || e.Contains("/"))) {
                Assert.False(new FilePath(path).IsWindows, path);
            }
        }

        [Test]
        public void CheckNames() {
            foreach (var name in NAMES) {
                var path = new FilePath(name.Key);
                Assert.True(path.Name == name.Value, $"{path.Name} != {name.Value}");
            }
        }

        [Test]
        public void CheckParents() {
            foreach (var name in PARENTS) {
                var path = new FilePath(name.Key);
                Assert.True((path.Parent?.Name ?? string.Empty) == (name.Value ?? string.Empty), 
                    $"{path.Parent} != {name.Value}"
                    );
            }
        }

        [Test]
        public void WindowsToUnix() {
            for (var i = 0; i < WINDOWS_PATHS.Count; i++) {
                var path = new FilePath(WINDOWS_PATHS[i]).ToUnix();
                Assert.True(path == WINDOWS_TO_UNIX_PATHS[i], $"{path} != {WINDOWS_TO_UNIX_PATHS[i]}");
            }
        }

        [Test]
        public void UnixToWindows() {
            for (var i = 0; i < UNIX_PATHS.Count; i++) {
                var path = new FilePath(UNIX_PATHS[i]).ToWindows();
                Assert.True(path == UNIX_TO_WINDOWS_PATHS[i], $"{path} != {UNIX_TO_WINDOWS_PATHS[i]}");
            }
        }

        [Test]
        public void EqualsString() {
            foreach (var path in VALID_PATHS) {
                var file = new FilePath(path);
                Assert.True(file.Equals(path), $"{file} != {path}");
            }
        }

        [Test]
        public void NotEqualsString() {
            foreach (var path in VALID_PATHS) {
                var file = new FilePath(path);
                Assert.False(file.Equals(path + "_"), $"{file} == {path}");
            }
        }

        [Test]
        public void Combine() {
            Assert.AreEqual("/home/user", new FilePath("/home") + new FilePath("user"));
            Assert.AreEqual("C:\\home\\user", "C:" + new FilePath("home") + new FilePath("user"));
        }
    }
}
