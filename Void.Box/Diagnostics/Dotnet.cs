using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Void.IO;
using Void.Collections;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Void.Diagnostics
{
    public static class Dotnet
    {
        private static readonly string SOLUTION_FILE_EXTENSION = "sln";
        private static readonly string PROJECT_FILE_EXTENSION = "csproj";
        private static readonly Regex PROJECT_SELECTOR = new Regex(@"""(?<PATH>[^""]+\.csproj)""");


        /// <summary>
        /// Extract all projects from the solution file.
        /// </summary>
        /// <param name="solution">Solution file.</param>
        /// <returns>Ordered file collection.</returns>
        public static IEnumerable<FileInfo> GetProjectsFromSolution(FileInfo solution) {
            if (solution == null) {
                throw new ArgumentNullException(nameof(solution));
            }
            using (var stream = File.Open(solution.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream)) {
                var content = reader.ReadToEnd();
                return PROJECT_SELECTOR
                    .Matches(content)
                    .Cast<Match>()
                    .Select(e => solution.Combine(e.Groups["PATH"].Value))
                    .OrderBy(e => e)
                    .Select(e => new FileInfo(e));
            }
        }

        /// <summary>
        /// Extract all projects from the solution file.
        /// </summary>
        /// <param name="solution">Solution file.</param>
        /// <returns>Ordered file collection.</returns>
        public static async Task<IEnumerable<FileInfo>> GetProjectsFromSolutionAsync(FileInfo solution) {
            if (solution == null) {
                throw new ArgumentNullException(nameof(solution));
            }
            using (var stream = File.Open(solution.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream)) {
                var content = await reader.ReadToEndAsync();
                return PROJECT_SELECTOR
                    .Matches(content)
                    .Cast<Match>()
                    .Select(e => solution.Combine(e.Groups["PATH"].Value))
                    .OrderBy(e => e)
                    .Select(e => new FileInfo(e));
            }
        }

        /// <summary>
        /// Find all project files in sub-folders and all parent folders. 
        /// Uses application entry point directory for start searching.
        /// </summary>
        /// <returns>Ordered file collection.</returns>
        public static IEnumerable<FileInfo> GetProjectsRecursive() {
            return GetSolutionsRecursive(Files.EntryPoint.Directory);
        }

        /// <summary>
        /// Find all project files in sub-folders and all parent folders.
        /// </summary>
        /// <param name="location">Start directory.</param>
        /// <returns>Ordered file collection.</returns>
        public static IEnumerable<FileInfo> GetProjectsRecursive(DirectoryInfo location) {
            if (location == null) {
                throw new ArgumentNullException(nameof(location));
            }
            var extension = $"*.{PROJECT_FILE_EXTENSION}";
            return GetFilesRecursiveUp(location, extension)
                .Union(GetFilesRecursiveDown(location, extension))
                .Select(e => e.FullName)
                .ToHashSet()
                .OrderBy(e => e)
                .Select(e => new FileInfo(e));
        }

        /// <summary>
        /// Find all solution files in sub-folders and all parent folders. 
        /// Uses application entry point directory for start searching.
        /// </summary>
        /// <returns>Ordered file collection.</returns>
        public static IEnumerable<FileInfo> GetSolutionsRecursive() {
            return GetSolutionsRecursive(Files.EntryPoint.Directory);
        }

        /// <summary>
        /// Find all solution files in sub-folders and all parent folders.
        /// </summary>
        /// <param name="location">Start directory.</param>
        /// <returns>Ordered file collection.</returns>
        public static IEnumerable<FileInfo> GetSolutionsRecursive(DirectoryInfo location) {
            if (location == null) {
                throw new ArgumentNullException(nameof(location));
            }
            var extension = $"*.{SOLUTION_FILE_EXTENSION}";
            return GetFilesRecursiveUp(location, extension)
                .Union(GetFilesRecursiveDown(location, extension))
                .Select(e => e.FullName)
                .ToHashSet()
                .OrderBy(e => e)
                .Select(e => new FileInfo(e));
        }

        private static HashSet<T> ToHashSet<T>(this IEnumerable<T> collection) {
            return new HashSet<T>(collection);
        }

        private static List<FileInfo> GetFilesRecursiveDown(DirectoryInfo location, string extension) {
            var files = location.GetFiles(extension).ToList();
            foreach (var directory in location.GetDirectories()) {
                files.AddRange(GetFilesRecursiveDown(location, extension));
            }
            return files;
        }

        private static List<FileInfo> GetFilesRecursiveUp(DirectoryInfo location, string extension) {
            var directory = location;
            var files = new List<FileInfo>();
            while (directory != null) {
                files.AddRange(directory.GetFiles(extension));
                directory = directory.Parent;
            }
            return files;
        }
    }
}
