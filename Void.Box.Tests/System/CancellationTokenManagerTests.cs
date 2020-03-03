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

        [Test]
        public void AlreadyCanceled() {
            using var cancel = new CancellationTokenManager();
            cancel.Cancel();
            Assert.ThrowsAsync<TaskCanceledException>(() => Task.Delay(100, cancel.Token));
            Assert.True(cancel.IsCancellationRequested);
            Assert.False(cancel.IsDisposed);
        }

        [Test]
        public void CancelAfter() {
            using var cancel = new CancellationTokenManager();
            var delay = Task.Delay(100, cancel.Token);
            cancel.CancelAfter(TimeSpan.FromMilliseconds(20));
            Assert.ThrowsAsync<TaskCanceledException>(async () => {
                await delay;
            });
            Assert.True(cancel.IsCancellationRequested);
            Assert.False(cancel.IsDisposed);
        }

        [Test]
        public void AlreadyDisposed() {
            var cancel = new CancellationTokenManager();
            cancel.Dispose();
            Assert.ThrowsAsync<TaskCanceledException>(() => Task.Delay(100, cancel.Token));
            Assert.True(cancel.IsCancellationRequested);
            Assert.True(cancel.IsDisposed);
        }

        [Test]
        public async Task DisposeExecuting() {
            var cancel = new CancellationTokenManager();
            var delay = Task.Delay(1000, cancel.Token);
            await Task.Delay(50);
            cancel.Dispose();
            Assert.ThrowsAsync<TaskCanceledException>(async () => await delay);
            Assert.True(cancel.IsCancellationRequested);
            Assert.True(cancel.IsDisposed);
        }
    }
}
