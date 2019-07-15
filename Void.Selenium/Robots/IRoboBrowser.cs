using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Void.Selenium
{
    public interface IRoboBrowser
    {
        /// <summary>
        /// Get screenshot of browser content as byte array.
        /// </summary>
        byte[] GetScreenshot();

        /// <summary>
        /// Check 'document.readyState' with JavaScript.
        /// </summary>
        /// <returns>True if 'document.readyState' is 'complete' else False.</returns>
        bool IsContentLoaded();

        /// <summary>
        /// Try to close current browser alert.
        /// </summary>
        /// <returns>True if alert is closed else False.</returns>
        bool CloseAlert();

        /// <summary>
        /// Try to get current browser alert and switch to it.
        /// </summary>
        /// <returns>Alert if found else null.</returns>
        IAlert GetAlert();
        void Scroll(int offset);

        /// <summary>
        /// Soft scrolling asynchronously by offset with 250 px / sec.
        /// </summary>
        /// <param name="offset">Positive or negative offset.</param>
        Task SoftScrollAsync(int offset);

        /// <summary>
        /// Soft scrolling asynchronously by offset with custom duration.
        /// </summary>
        /// <param name="offset">Positive or negative offset.</param>
        Task SoftScrollAsync(int offset, TimeSpan duration);

        /// <summary>
        /// Soft scrolling asynchronously by offset with custom duration.
        /// </summary>
        /// <param name="offset">Positive or negative offset.</param>
        /// <exception cref="TaskCanceledException">The task has been canceled.</exception>
        Task SoftScrollAsync(int offset, TimeSpan duration, CancellationToken token);

        /// <summary>
        /// Close alerts, go back and wait content loading asynchronously
        /// with robot's page searching timeout.
        /// </summary>
        /// <exception cref="TimeoutException">Page os not loaded.</exception>
        Task BackAsync();

        /// <summary>
        /// Close alerts, go back and wait content loading asynchronously.
        /// </summary>
        /// <exception cref="TimeoutException">Page os not loaded.</exception>
        Task BackAsync(TimeSpan timeout);

        /// <summary>
        /// Close alerts, go back and wait content loading asynchronously.
        /// </summary>
        /// <exception cref="TimeoutException">Page os not loaded.</exception>
        /// <exception cref="TaskCanceledException">The task has been canceled.</exception>
        Task BackAsync(TimeSpan timeout, CancellationToken token);

        Task WaitContentLoadingAsync();
        Task WaitContentLoadingAsync(TimeSpan timeout);
        Task WaitContentLoadingAsync(TimeSpan timeout, CancellationToken token);
    }
}
