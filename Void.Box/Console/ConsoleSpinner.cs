using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Void
{
    class ConsoleSpinner : IConsoleSpinner
    {
        private static readonly string STATES = @"/-\|";

        private readonly CancellationTokenManager cancel;
        private readonly object locker;
        private bool started;
        private int counter;


        public bool Clockwise { get; set; }
        public TimeSpan Interval { get; set; }


        public ConsoleSpinner() {
            this.cancel = new CancellationTokenManager();
            this.locker = new object();
        }


        public void Start() {
            lock (this.locker) {
                if (this.started) {
                    return;
                }
                this.started = true;
                Task.Run(RotateAsync);
            }
        }

        public void Dispose() {
            this.cancel.Dispose();
        }

        private void Rotate() {
            this.counter++;
            var reverse = this.Clockwise ? 0 : STATES.Length;
            Console.Write(STATES[Math.Abs(reverse - counter % STATES.Length)]);
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }

        private async Task RotateAsync() {
            while (!this.cancel.Token.IsCancellationRequested) {
                try {
                    Rotate();
                    await Task.Delay(this.Interval, this.cancel.Token);
                }
                catch (OperationCanceledException) {
                    return;
                }
            }
        }
    }
}
