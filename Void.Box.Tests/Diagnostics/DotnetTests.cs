using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Void.IO;

namespace Void.Diagnostics
{
    [Parallelizable]
    public class DotnetTests
    {
        [Test]
        public void GetSolutionsRecursive() {
            Assert.That(Dotnet.GetSolutionsRecursive().Any(e => e.Name == "Void.sln"), "Current solution is not found");
        }

        [Test]
        public void GetProjectsRecursive() {
            var projects = new string[] { 
                "Void.Box.Tests",
            };
            var files = Dotnet.GetProjectsRecursive();
            foreach (var project in projects) {
                Assert.That(files.Any(e => Path.GetFileNameWithoutExtension(e.Name) == project), 
                    $"Project is not found: {project}"
                    );
            }
        }

        [Test]
        public async Task GetProjectsFromSolution() {
            var projects = new string[] {
                "Void.Box",
                "Void.Json",
                "Void.Box.Tests",
                "Void.Test.Printer"
            };
            var solution = Dotnet
                .GetSolutionsRecursive()
                .FirstOrDefault(e => Path.GetFileNameWithoutExtension(e.Name) == "Void")
                ?? throw new FileNotFoundException("Solution is not found");
            var files = await Dotnet.GetProjectsFromSolutionAsync(solution);
            foreach (var project in projects) {
                Assert.That(files.Any(e => Path.GetFileNameWithoutExtension(e.Name) == project),
                    $"Project is not found: {project}"
                    );
            }
        }

        [Test]
        public async Task Publish() {
            var solution = Dotnet
                .GetSolutionsRecursive()
                .FirstOrDefault(e => Path.GetFileNameWithoutExtension(e.Name) == "Void")
                ?? throw new FileNotFoundException("Solution is not found");
            using var location = new TempDirectory();
            var projects = await Dotnet.GetProjectsFromSolutionAsync(solution);
            var printerProject = projects.First(e => Path.GetFileNameWithoutExtension(e.Name) == "Void.Test.Printer");
            var artefacts = await Dotnet.PublishAsync(printerProject.FullName, ProjectConfiguration.Release, location.Value);
            Assert.AreEqual("Void.Test.Printer.dll", artefacts.EntryPoint.Name);
            var settings = artefacts.GetStartInfo();
            settings.RedirectStandardOutput = true;
            settings.RedirectStandardError = true;
            settings.UseShellExecute = false;
            settings.CreateNoWindow = true;
            settings.Arguments += " \"pew pew\"";
            using var process = Process.Start(settings);
            using var cancel = new CancellationTokenSource();
            cancel.CancelAfter(TimeSpan.FromSeconds(5));
            var outputReader = process.StandardOutput.ReadToEndAsync();
            var errorReader = process.StandardError.ReadToEndAsync();
            try {
                await process.WaitForExitAsync(cancel.Token);
            }
            catch (OperationCanceledException) {
                throw new TimeoutException("Printer is frozen");
            }
            finally {
                try {
                    process.Kill();
                }
                catch { }
            }
            var outputMessage = await outputReader;
            var errorMessage = await errorReader;
            Assert.AreEqual(0, process.ExitCode, $"Process extic with {process.ExitCode} code. {errorMessage}");
            Assert.That(outputMessage.Contains("pew pew"));
        }
    }
}
