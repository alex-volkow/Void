using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Void.IO;
using Void.Collections;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace Void.Diagnostics
{
    public static class Dotnet
    {
        private static readonly string SOLUTION_FILE_EXTENSION = "sln";
        private static readonly string PROJECT_FILE_EXTENSION = "csproj";
        private static readonly Regex PROJECT_SELECTOR = new Regex(@"""(?<PATH>[^""]+\.csproj)""");


        /// <summary>
        /// Get version of the entry point assembly.
        /// </summary>
        public static string EntryPointVersion => Assembly
            .GetEntryAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;


        /// <summary>
        /// Get existing *.exe file for the type assembly ot null.
        /// </summary>
        /// <returns></returns>
        public static FileInfo GetExecutable(Type type) {
            var dll = new FileInfo(type.Assembly.Location);
            var exe = dll.Directory.Combine($"{Path.GetFileNameWithoutExtension(dll.FullName)}.exe");
            return File.Exists(exe) ? new FileInfo(exe) : null;
        }

        /// <summary>
        /// Build the project to the output directory.
        /// </summary>
        /// <param name="project">Name or path.</param>
        /// <param name="config">Artefacts configuration.</param>
        /// <param name="output">Artefacts directory.</param>
        /// <param name="token">Indicates the task is canceled.</param>
        public static Task<IProjectArtefacts> BuildAsync(
            string project, 
            ProjectConfiguration config, 
            DirectoryInfo output,
            CancellationToken token = default
            ) {
            return CreateArtefactsAsync(project, "build", config, output, token);
        }

        /// <summary>
        /// Publish the project to the output directory.
        /// </summary>
        /// <param name="project">Name or path.</param>
        /// <param name="config">Artefacts configuration.</param>
        /// <param name="output">Artefacts directory.</param>
        /// <param name="token">Indicates the task is canceled.</param>
        public static Task<IProjectArtefacts> PublishAsync(
            string project,
            ProjectConfiguration config,
            DirectoryInfo output,
            CancellationToken token = default
            ) {
            return CreateArtefactsAsync(project, "publish", config, output, token);
        }


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
                    .Select(e => solution.Directory.Combine(e.Groups["PATH"].Value))
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
            return GetProjectsRecursive(Files.EntryPoint.Directory);
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
                files.AddRange(GetFilesRecursiveDown(directory, extension));
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

        private static FileInfo GetRequiredProject(string name) {
            if (string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException(
                    $"Project name required"
                    );
            }
            var project = AddProjectExtension(name).Trim('\\', '/', ' ', '\t', '\r', '\n');
            return GetProjectFromDirectory(project)
                ?? GetProjectFromDirectory(project, Files.EntryPoint.Directory)
                ?? GetProjectFromDirectory(project, new DirectoryInfo(Directory.GetCurrentDirectory()))
                ?? throw new FileNotFoundException("Project file is not found");
        }

        private static FileInfo GetProjectFromDirectory(string project, DirectoryInfo location = default) {
            var path = project;
            if (location != null && Path.IsPathRooted(project)) {
                path = location.Combine(project);
            }
            if (File.Exists(path)) {
                return new FileInfo(path);
            }
            var directory = RemoveProjectExtension(path);
            if (Directory.Exists(directory)) {
                var projects = Directory.GetFiles(directory, $"*.{PROJECT_FILE_EXTENSION}");
                var name = Path.GetFileNameWithoutExtension(path);
                foreach (var file in projects) {
                    var filename = Path.GetFileNameWithoutExtension(file);
                    if (filename == name) {
                        return new FileInfo(file);
                    }
                }
                if (projects.Length == 1) {
                    return new FileInfo(projects.First());
                }
            }
            return null;
        }

        private static string AddProjectExtension(string name) {
            return !name.EndsWith($".{PROJECT_FILE_EXTENSION}", StringComparison.OrdinalIgnoreCase)
                ? $"{name}.{PROJECT_FILE_EXTENSION}"
                : name;
        }

        private static string RemoveProjectExtension(string path) {
            return path.Replace($".{PROJECT_FILE_EXTENSION}", string.Empty);
        }

        private static async Task<IProjectArtefacts> CreateArtefactsAsync(
            string project,
            string command,
            ProjectConfiguration config,
            DirectoryInfo output,
            CancellationToken token
            ) {
            if (output == null) {
                throw new ArgumentNullException(nameof(output));
            }
            var projectFile = GetRequiredProject(project);
            var arguments = new StringBuilder();
            arguments.Append(command).Append(" ");
            arguments.Append("--configuration").Append(" ").Append(config).Append(" ");
            arguments.Append("--output").Append(" \"").Append(output.FullName).Append("\"");
            using (var process = new Process()) {
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = arguments.ToString();
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.WorkingDirectory = projectFile.DirectoryName;
                process.Start();
                var outputReader = process.StandardOutput.ReadToEndAsync();
                var errorReader = process.StandardError.ReadToEndAsync();
                using (token.Register(process.Kill)) {
                    await process.WaitForExitAsync(token);
                }
                var outputMessage = await outputReader;
                var errorMessage = await errorReader;
                if (process.ExitCode != default) {
                    var message = new StringBuilder();
                    message.Append("Process have been completed with ");
                    message.Append(process.ExitCode);
                    message.Append(" code.");
                    if (!string.IsNullOrWhiteSpace(errorMessage)) {
                        message.AppendLine();
                        message.Append(errorMessage.Trim());
                        throw new InvalidOperationException(
                            message.ToString()
                            );
                    }
                    if (!string.IsNullOrWhiteSpace(outputMessage)) {
                        message.AppendLine();
                        message.Append(outputMessage.Trim());
                        throw new InvalidOperationException(
                            message.ToString()
                            );
                    }
                    throw new InvalidOperationException(
                        message.ToString()
                        );
                }
                return new ProjectArtefacts(projectFile, output);
            }
        }
    }
}
