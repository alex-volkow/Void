using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Void.IO;

namespace Void.Diagnostics
{
    /// <summary>
    /// Represents API to access to project artefacts.
    /// </summary>
    public interface IProjectArtefacts
    {
        /// <summary>
        /// Project file.
        /// </summary>
        FileInfo Project { get; }

        /// <summary>
        /// Application entry point file or null.
        /// </summary>
        FileInfo EntryPoint { get; }

        /// <summary>
        /// Location of artefacts files.
        /// </summary>
        DirectoryInfo Location { get; }


        /// <summary>
        /// Create new isntance of ProcessStartInfo with entry point.
        /// </summary>
        /// <exception cref="InvalidOperationException">No entry point.</exception>
        ProcessStartInfo GetStartInfo();

        /// <summary>
        /// Get artefact files recursive.
        /// </summary>
        IEnumerable<FileInfo> GetFiles();

        /// <summary>
        /// Get artefact entries recursive.
        /// </summary>
        IEnumerable<IEntryReader> GetEntries();

        /// <summary>
        /// Fend all *.exe files in the artefacts location.
        /// </summary>
        /// <returns></returns>
        IEnumerable<FileInfo> GetExecutableFiles();

        /// <summary>
        /// Delete the artefact location directory with all content.
        /// </summary>
        void Delete();
    }
}
