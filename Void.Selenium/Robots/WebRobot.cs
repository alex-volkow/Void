using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Void.Selenium
{
    public class WebRobot : IRobot
    {
        /// <summary>
        /// Wrapped driver
        /// </summary>
        public IWebDriver WrappedDriver { get; }

        protected IJavaScriptExecutor JavaScript => (IJavaScriptExecutor)this.WrappedDriver;

        public TimeSpan KeySendingInterval => throw new NotImplementedException();

        public TimeSpan PageSearchingTimeout => throw new NotImplementedException();

        public TimeSpan ElementSearchingTimeout => throw new NotImplementedException();

        public TimeSpan ConditionWaitingTimeout => throw new NotImplementedException();

        public TimeSpan ConditionCheckingInterval => throw new NotImplementedException();

        public double RandomWaitDeviationPercent => throw new NotImplementedException();

        public IRoboElements Elements => throw new NotImplementedException();

        public IRoboBrowser Browser => throw new NotImplementedException();

        public IRoboPages Pages => throw new NotImplementedException();

        public WebRobot(IWebDriver driver) {
            this.WrappedDriver = driver ?? throw new ArgumentNullException(nameof(driver));
        }


        /// <summary>
        /// Check driver's page content is loaded with JavaScript ('document.readyState' is 'complete')
        /// </summary>
        public bool IsContentLoaded() {
            return this.JavaScript.ExecuteScript("return document.readyState") is "complete";
        }

        /// <summary>
        /// Get screenshot of current driver's page as byte array.
        /// </summary>
        public byte[] GetScreenshot() {
            return ((ITakesScreenshot)this.WrappedDriver).GetScreenshot().AsByteArray;
        }

        public IRoboElement Using(IWebElement element) {
            throw new NotImplementedException();
        }

        public IRoboElement Using(IWebPointer pointer) {
            throw new NotImplementedException();
        }

        public IRoboElement Using(string xpath) {
            throw new NotImplementedException();
        }

        public IRoboElement Using(By locator) {
            throw new NotImplementedException();
        }

        public object ExecuteJavaScript(string script) {
            throw new NotImplementedException();
        }

        public Task<bool> WaitAsync(Func<bool> condition) {
            throw new NotImplementedException();
        }

        public Task<bool> WaitAsync(Func<bool> condition, TimeSpan timeout) {
            throw new NotImplementedException();
        }

        public Task<bool> WaitAsync(Func<bool> condition, TimeSpan timeout, TimeSpan interval) {
            throw new NotImplementedException();
        }

        public Task WaitRandomAsync(Delays delay) {
            throw new NotImplementedException();
        }

        public Task WaitRandomAsync(TimeSpan delay) {
            throw new NotImplementedException();
        }

        public Task WaitRandomAsync(int milliseconds) {
            throw new NotImplementedException();
        }

        public Task WaitRandomAsync(Delays from, Delays to) {
            throw new NotImplementedException();
        }

        public Task WaitRandomAsync(TimeSpan from, TimeSpan to) {
            throw new NotImplementedException();
        }

        public Task WaitRandomAsync(int fromMs, int toMs) {
            throw new NotImplementedException();
        }

        public Task WaitContentLoadingAsync() {
            throw new NotImplementedException();
        }

        public Task WaitContentLoadingAsync(TimeSpan timeout) {
            throw new NotImplementedException();
        }
    }
}
