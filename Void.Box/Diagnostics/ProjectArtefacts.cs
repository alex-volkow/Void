using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Void.IO;

namespace Void.Diagnostics
{
    class ProjectArtefacts : IProjectArtefacts
    {
        public FileInfo Project => throw new NotImplementedException();

        public FileInfo EntryPoint => throw new NotImplementedException();

        public DirectoryInfo Location => throw new NotImplementedException();


        public ProjectArtefacts(FileInfo project, DirectoryInfo location) {

        }

        public void Delete() {
            throw new NotImplementedException();
        }

        public void Dispose() {
            throw new NotImplementedException();
        }

        public IEnumerable<IEntryReader> GetEntries() {
            throw new NotImplementedException();
        }

        public IEnumerable<FileInfo> GetExecutableFiles() {
            throw new NotImplementedException();
        }

        public IEnumerable<FileInfo> GetFiles() {
            throw new NotImplementedException();
        }

        public Process Start(params string[] arguments) {
            throw new NotImplementedException();
        }

        public Process Start(IEnumerable<string> arguments) {
            throw new NotImplementedException();
        }

        public Process Start(ProcessStartInfo settings) {
            throw new NotImplementedException();
        }
    }
}
