﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Void.IO;

namespace Void.Diagnostics
{
    class ProjectArtefacts : IProjectArtefacts
    {
        private static readonly string RUNTIME_CONFIG_EXTENSION = "runtimeconfig.json";
        private static readonly string EXECUTABLE_FILE_EXTENSION = "dll";

        private readonly Lazy<FileInfo> entryPoint;


        public FileInfo Project { get; }

        public FileInfo EntryPoint => this.entryPoint.Value;

        public DirectoryInfo Location { get; }



        public ProjectArtefacts(FileInfo project, DirectoryInfo location) {
            this.Location = location ?? throw new ArgumentNullException(nameof(location));
            this.Project = project ?? throw new ArgumentNullException(nameof(project));
            this.entryPoint = new Lazy<FileInfo>(GetEntryPoint);
        }



        public ProcessStartInfo GetStartInfo() {
            if (this.EntryPoint == null) {
                throw new InvalidOperationException(
                    $"Artefacts have no entry point"
                    );
            }
            var settings = new ProcessStartInfo {
                WorkingDirectory = this.EntryPoint.DirectoryName,
                Arguments = $"\"{this.EntryPoint.FullName}\"",
                FileName = "dotnet",
            };
            return settings;
        }

        public void Delete() {
            this.Location.DeleteWithContent();
        }

        public void Dispose() {
            Delete();
        }

        public IEnumerable<IEntryReader> GetEntries() {
            return this.Location
                .GetContent()
                .Select(e => new FileEntry(this.Location, e));
        }

        public IEnumerable<FileInfo> GetExecutableFiles() {
            return GetFiles().Where(e => e.Extension.TrimStart('.') == EXECUTABLE_FILE_EXTENSION);
        }

        public IEnumerable<FileInfo> GetFiles() {
            return this.Location.GetContent();
        }

        private FileInfo GetEntryPoint() {
            foreach (var file in this.Location.GetFiles()) {
                if (file.Name.EndsWith(RUNTIME_CONFIG_EXTENSION)) {
                    var path = file.FullName.Replace(RUNTIME_CONFIG_EXTENSION, EXECUTABLE_FILE_EXTENSION);
                    var executable = new FileInfo(path);
                    if (executable.Exists) {
                        return executable;
                    }
                }
            }
            return null;
        }
    }
}
