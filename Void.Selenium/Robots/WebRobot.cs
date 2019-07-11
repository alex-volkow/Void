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



        public WebRobot(IWebDriver driver) {
            this.WrappedDriver = driver ?? throw new ArgumentNullException(nameof(driver));
        }


        /// <summary>
        /// Check driver's page content is loaded with JavaScript ('document.readyState' is 'complete')
        /// </summary>
        public bool IsContentLoaded() {
            return this.JavaScript.ExecuteScript("return document.readyState") is "complete";
        }

        public Task WaitContentLoadingAsync() {
            throw new NotImplementedException();
        }

        public Task WaitContentLoadingAsync(TimeSpan timeout) {
            //var wait = new DefaultWait<IJavaScriptExecutor>(driver.ToJavaScriptExecutor()) {
            //    Timeout = timeout
            //};
            //wait.Until(e => e.ExecuteScript("return document.readyState") is "complete");
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get screenshot of current driver's page as byte array.
        /// </summary>
        public byte[] GetScreenshot() {
            return ((ITakesScreenshot)this.WrappedDriver).GetScreenshot().AsByteArray;
        }
    }
}
