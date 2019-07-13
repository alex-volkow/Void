using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Void.Selenium
{
    class RoboBrowser : RoboComponent, IRoboBrowser
    {
        public RoboBrowser(IRobot robot)
            : base(robot) {
        }

        public Task BackAsync() {
            throw new NotImplementedException();
        }

        public bool CloseAlert() {
            throw new NotImplementedException();
        }

        public IAlert GetAlert() {
            throw new NotImplementedException();
        }

        public byte[] GetScreenshot() {
            return ((ITakesScreenshot)this.WrappedDriver).GetScreenshot().AsByteArray;
        }

        public bool IsContentLoaded() {
            return this.Robot.ExecuteJavaScript("return document.readyState") is "complete";
        }

        public void Scroll(int offset) {
            throw new NotImplementedException();
        }

        public Task SoftScrollAsync(int offset) {
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
