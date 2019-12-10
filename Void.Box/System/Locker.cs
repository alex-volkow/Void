using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Void
{
    /// <summary>
    /// Represents one line semaphore.
    /// </summary>
    public sealed class Locker : IDisposable
    {
        private readonly SemaphoreSlim sync;
        private readonly object locker;
        private bool disposed;


        public Locker() {
            this.sync = new SemaphoreSlim(1, 1);
            this.locker = new object();
        }


        public void Dispose() {
            lock (this.locker) {
                if (this.disposed) {
                    this.disposed = true;
                    this.sync.Dispose();
                }
            }
        }

        public IDisposable Lock() {
            this.sync.Wait();
            return new Disposer(this.sync);
        }

        public async Task<IDisposable> LockAsync() {
            await this.sync.WaitAsync();
            return new Disposer(this.sync);
        }

        public async Task<IDisposable> LockAsync(CancellationToken token) {
            await this.sync.WaitAsync(token);
            return new Disposer(this.sync);
        }

        private class Disposer : IDisposable
        {
            private readonly SemaphoreSlim sync;
            private readonly object locker;
            private bool disposed;


            public Disposer(SemaphoreSlim sync) {
                this.locker = new object();
                this.sync = sync;
            }

            public void Dispose() {
                lock (this.locker) {
                    if (!this.disposed) {
                        this.disposed = true;
                        this.sync.Release();
                    }
                }
            }
        }
    }
}
