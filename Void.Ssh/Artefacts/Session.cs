using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Void.Net
{
    class Session<T> : ISession<T> where T : BaseClient
    {
        private readonly object locker;
        private readonly AutoClient<T> client;
        private TaskCompletionSource<bool> result;


        public AutoClient<T> Client => this.client;

        public bool IsActive {
            get {
                lock (this.locker) {
                    return this.result != null 
                        && !this.result.Task.IsCompleted;
                }
            }
        }

        T ISession<T>.Client => this.client.Client;



        public Session(T client) {
            this.client = new AutoClient<T>(client ?? throw new ArgumentNullException(nameof(client)));
            this.locker = new object();
        }

        public void Activate() {
            lock (this.locker) {
                if (this.IsActive) {
                    throw new InvalidOperationException(
                        $"Session is already activated"
                        );
                }
                this.result = new TaskCompletionSource<bool>();
            }
        }

        public void Dispose() {
            lock (this.locker) {
                if (this.IsActive) {
                    this.result.SetResult(true);
                }
            }
        }

        public Task GetAwaiter() {
            lock (this.locker) {
                return this.result?.Task ?? Task.CompletedTask;
            }
        }

        Task ISession<T>.GetAwaiter() {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose() {
            throw new NotImplementedException();
        }
    }
}
