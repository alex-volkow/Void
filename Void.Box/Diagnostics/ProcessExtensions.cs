using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Void.Diagnostics
{
    public static class ProcessExtensions
    {
        /// <summary>
        /// Wait for exit associated process with cancellation token.
        /// </summary>
        /// <exception cref="OperationCanceledException">Operation has been canceled.</exception>
        public static async Task WaitForExitAsync(
            this Process process,
            CancellationToken cancellationToken = default
            ) {
            var pointer = new TaskCompletionSource<bool>();
            void ProcessExited(object sender, EventArgs e) => Task.Run(() => pointer.TrySetResult(true));
            process.EnableRaisingEvents = true;
            process.Exited += ProcessExited;
            try {
                if (process.HasExited) {
                    return;
                }
                using (cancellationToken.Register(() => Task.Run(() => pointer.TrySetCanceled()))) {
                    await pointer.Task;
                }
            }
            finally {
                process.Exited -= ProcessExited;
            }
        }
    }
}
