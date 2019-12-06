using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Void.IO
{
    /// <summary>
    /// Represents auto removable temp file.
    /// </summary>
    public sealed class TempFile : IDisposable
    {
        public FileInfo Info { get; }



        public TempFile() : this(Path.GetTempFileName()) { }

        public TempFile(string path) : this(new FileInfo(path)) { }

        public TempFile(FileInfo file) {
            this.Info = file ?? throw new ArgumentNullException(nameof(file));
        }

        ~TempFile() {
            Dispose(false);
        }



        public void Dispose() {
            Dispose(true);
        }

        private void Dispose(bool disposing) {
            if (disposing) {
                GC.SuppressFinalize(this);
            }
            if (File.Exists(this.Info.FullName)) {
                try { File.Delete(this.Info.FullName); }
                catch { }
            }
        }
    }
}
