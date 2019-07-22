using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Void.Net
{
    public class Adapter<T> : IDisposable where T : BaseClient
    {
        public static TimeSpan DefaultConnectionInterval { get; } = TimeSpan.FromSeconds(5);


        private readonly object locker;
        private TimeSpan connectionInterval;
        private bool reconnecting;
        private bool autoconnect;


        public T Client { get; }

        public bool IsAutoConnect {
            get {
                lock (this.locker) {
                    return this.autoconnect;
                }
            }
        }

        public TimeSpan ConnectionInterval {
            get {
                lock (this.locker) {
                    return this.connectionInterval;
                }
            }
        }



        public Adapter(T client) {
            this.Client = client ?? throw new ArgumentNullException(nameof(client));
            this.locker = new object();
        }



        public void AutoConnect() {
            AutoConnect(DefaultConnectionInterval);
        }

        public void AutoConnect(TimeSpan interval) {
            lock (this.locker) {
                this.connectionInterval = interval.Duration();
                if (!this.IsAutoConnect) {
                    this.autoconnect = true;
                    this.Client.ErrorOccurred += HandleError;
                }
            }
            TryConnect();
        }

        public void Disconnect() {
            lock (this.locker) {
                if (this.autoconnect) {
                    this.autoconnect = false;
                    this.Client.ErrorOccurred -= HandleError;
                }
            }
            if (this.Client.IsConnected) {
                this.Client.Disconnect();
            }
        }

        public virtual void Dispose() {
            Disconnect();
            this.Client.Dispose();
        }

        public override int GetHashCode() {
            return HashCode.Create(
                GetType(),
                this.Client.ConnectionInfo.Port,
                this.Client.ConnectionInfo.Host
                );
        }

        public override string ToString() {
            return this.Client.ConnectionInfo.Port != 22
                ? $"{this.Client.ConnectionInfo.Host}:{this.Client.ConnectionInfo.Port}"
                : this.Client.ConnectionInfo.Host;
        }

        protected virtual void Reconnect() {
            this.Client.Connect();
        }

        private void HandleError(object sender, ExceptionEventArgs e) {
            TryConnect();
        }

        private async void TryConnect() {
            lock (this.locker) {
                if (this.reconnecting) {
                    return;
                }
                this.reconnecting = true;
            }
            try {
                while (!this.Client.IsConnected && this.IsAutoConnect) {
                    try {
                        Reconnect();
                    }
                    catch (Exception ex) {
                        if (ex is ObjectDisposedException) {
                            Disconnect();
                            return;
                        }
                        await Task.Delay(this.ConnectionInterval);
                    }
                }
            }
            catch { }
            finally {
                lock (this.locker) {
                    this.reconnecting = false;
                }
            }
        }
    }
}
