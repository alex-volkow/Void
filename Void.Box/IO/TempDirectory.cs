using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Void.IO
{
    public sealed class TempDirectory : IDisposable
    {
        public DirectoryInfo Value { get; }



        public TempDirectory() : this(Files.CreateTempDirectory(Guid.NewGuid().ToString())) { }

        public TempDirectory(string path) : this(new DirectoryInfo(path)) { }

        public TempDirectory(DirectoryInfo directory) {
            this.Value = directory ?? throw new ArgumentNullException(nameof(directory));
        }

        ~TempDirectory() {
            Dispose(false);
        }



        public void Dispose() {
            Dispose(true);
        }

        private void Dispose(bool disposing) {
            if (disposing) {
                GC.SuppressFinalize(this);
            }
            if (Directory.Exists(this.Value.FullName)) {
                this.Value.TryDeleteWithContent();
            }
        }
    }
}
