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
        /// Default interval between keys values sending (175 ms).
        /// </summary>
        public static TimeSpan DefaultKeySendingInterval { get; } = TimeSpan.FromMilliseconds(175);

        /// <summary>
        /// Default page searching timeout (1 min).
        /// </summary>
        public static TimeSpan DefaultPageSearchingTimeout { get; } = TimeSpan.FromSeconds(60);

        /// <summary>
        /// Default element searching timeout (30 sec).
        /// </summary>
        public static TimeSpan DefaultElementSearchingTimeout { get; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Default waiting timeout (1 min).
        /// </summary>
        public static TimeSpan DefaultConditionWaitingTimeout { get; } = TimeSpan.FromSeconds(60);

        /// <summary>
        /// Default interval between condition checking while waiting (1 sec).
        /// </summary>
        public static TimeSpan DefaultConditionCheckingInterval { get; } = TimeSpan.FromMilliseconds(1000);

        /// <summary>
        /// Default time deviation for waiting (25 %).
        /// </summary>
        public static double DefaultRandomWaitDeviationPercent { get; } = 0.25;



        /// <summary>
        /// Wrapped driver.
        /// </summary>
        public IWebDriver WrappedDriver { get; }

        /// <summary>
        /// Provides access to Robot's elements.
        /// </summary>
        public IRoboElements Elements => new RoboElements(this);

        /// <summary>
        /// Provides access to Robot's browser.
        /// </summary>
        public IRoboBrowser Browser => new RoboBrowser(this);

        /// <summary>
        /// Provides access to Robot's pages.
        /// </summary>
        public IRoboPages Pages => new RoboPages(this);



        public TimeSpan KeySendingInterval { get; set; }

        public TimeSpan PageSearchingTimeout { get; set; }

        public TimeSpan ElementSearchingTimeout { get; set; }

        public TimeSpan ConditionWaitingTimeout { get; set; }

        public TimeSpan ConditionCheckingInterval { get; set; }

        public double RandomWaitDeviationPercent { get; set; }


        /// <summary>
        /// Cast WebDriver to IJavaScriptExecutor.
        /// </summary>
        protected IJavaScriptExecutor JavaScript => (IJavaScriptExecutor)this.WrappedDriver;



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
