using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Void
{
    /// <summary>
    /// Represents disposing safe variation of  CancellationTokenSource.
    /// </summary>
    public class CancellationTokenManager : IDisposable
    {
        private readonly CancellationTokenSource source;
        private readonly IDisposable callback;
        private readonly object locker;
        private bool disposed;

        /// <summary>
        /// Indicates the object is disposed.
        /// </summary>
        public bool IsDisposed {
            get {
                lock (this.locker) {
                    return this.disposed;
                }
            }
        }

        /// <summary>
        /// True if cancellation requested or object is disposed else False.
        /// </summary>
        public bool IsCancellationRequested {
            get {
                try {
                    lock (this.locker) {
                        return this.source.IsCancellationRequested;
                    }
                }
                catch (ObjectDisposedException) {
                    return true;
                }
            }
        }

        /// <summary>
        /// Gets the cancellation token associated with object.
        /// If the object is disposed, token alway be canceled.
        /// </summary>
        public CancellationToken Token {
            get {
                try {
                    lock (this.locker) {
                        return this.source.Token;
                    }
                }
                catch (ObjectDisposedException) {
                    return new CancellationToken(true);
                }
            }
        }


        public event EventHandler<EventArgs> Canceled;


        public CancellationTokenManager() {
            this.source = new CancellationTokenSource();
            this.locker = new object();
            this.callback = this.source.Token.Register(CancelCallback);
        }

        public CancellationTokenManager(int millisecondsDelay) {
            this.source = new CancellationTokenSource(millisecondsDelay);
            this.locker = new object();
            this.callback = this.source.Token.Register(CancelCallback);
        }

        public CancellationTokenManager(TimeSpan delay) {
            this.source = new CancellationTokenSource(delay);
            this.locker = new object();
            this.callback = this.source.Token.Register(CancelCallback);
        }

        public CancellationTokenManager(params CancellationToken[] tokens) {
            this.source = CancellationTokenSource.CreateLinkedTokenSource(tokens);
            this.locker = new object();
            this.callback = this.source.Token.Register(CancelCallback);
        }


        public void Cancel() {
            try {
                this.source.Cancel();
            }
            catch (ObjectDisposedException) {
                return;
            }
        }

        public void Cancel(bool throwOnFirstException) {
            try {
                this.source.Cancel(throwOnFirstException);
            }
            catch (ObjectDisposedException) {
                return;
            }
        }

        public void CancelAfter(int millisecondsDelay) {
            try {
                this.source.CancelAfter(millisecondsDelay);
            }
            catch (ObjectDisposedException) {
                return;
            }
        }

        public void CancelAfter(TimeSpan delay) {
            try {
                this.source.CancelAfter(delay);
            }
            catch (ObjectDisposedException) {
                return;
            }
        }

        public virtual void Dispose() {
            lock (this.locker) {
                if (!this.disposed) {
                    this.disposed = true;
                    if (!this.source.IsCancellationRequested) {
                        this.source.Cancel();
                    }
                    this.callback.Dispose();
                    this.source.Dispose();
                    this.Canceled = null;
                }
            }
        }

        private void CancelCallback() {
            this.Canceled?.Invoke(this, new EventArgs());
        }
    }
}
