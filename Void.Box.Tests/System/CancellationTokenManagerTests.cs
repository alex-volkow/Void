using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Void
{
    [Parallelizable]
    public class CancellationTokenManagerTests
    {
        [Test]
        public async Task NormalExecuting() {
            using var cancel = new CancellationTokenManager();
            await Task.Delay(100, cancel.Token);
            Assert.False(cancel.IsCancellationRequested);
            Assert.False(cancel.IsDisposed);
        }

        [Test]
        public void CancelExecuting() {
            using var cancel = new CancellationTokenManager();
            var delay = Task.Delay(100, cancel.Token);
            cancel.Cancel();
            Assert.ThrowsAsync<TaskCanceledException>(async () => {
                await delay;
            });
            Assert.True(cancel.IsCancellationRequested);
            Assert.False(cancel.IsDisposed);
        }
    }
}
