using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Void.IO;

namespace Void.Diagnostics
{
    public interface IProjectArtefacts : IDisposable
    {
        FileInfo Project { get; }
        FileInfo EntryPoint { get; }
        DirectoryInfo Location { get; }

        Process Start(params string[] arguments);
        Process Start(IEnumerable<string> arguments);
        Process Start(ProcessStartInfo settings);
        IEnumerable<FileInfo> GetFiles();
        IEnumerable<IEntryReader> GetEntries();
        IEnumerable<FileInfo> GetExecutableFiles();
        void Delete();
    }
}
