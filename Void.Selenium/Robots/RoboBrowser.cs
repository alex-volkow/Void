using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
            return BackAsync(this.Robot.PageSearchingTimeout);
        }

        public Task BackAsync(TimeSpan timeout) {
            return BackAsync(timeout, CancellationToken.None);
        }

        public Task BackAsync(TimeSpan timeout, CancellationToken token) {
            CloseAlert();
            this.WrappedDriver.Navigate().Back();
            return WaitContentLoadingAsync(timeout, token);
        }

        public bool CloseAlert() {
            var handler = this.WrappedDriver.CurrentWindowHandle;
            try {
                var alert = GetAlert();
                if (alert != null) {
                    alert.Dismiss();
                    return true;
                }
                return false;
            }
            catch {
                return false;
            }
            finally {
                this.WrappedDriver.SwitchTo().Window(handler);
            }
        }

        public IAlert GetAlert() {
            var handler = this.WrappedDriver.CurrentWindowHandle;
            try {
                var alert = this.WrappedDriver.SwitchTo().Alert();
                if (alert == null) {
                    this.WrappedDriver.SwitchTo().Window(handler);
                }
                return alert;
            }
            catch (NoAlertPresentException) {
                this.WrappedDriver.SwitchTo().Window(handler);
                return null;
            }
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
            //var sign = offset >= 0 ? 1 : -1;
            //var shift = 125; // 25
            //var delay = 250;
            //var script = $"window.scrollBy(0,{sign * shift})";
            //var limit = Math.Abs(offset);
            //for (var i = 0; i < limit; i += shift) {
            //    this.Robot.ExecuteJavaScript(script);
            //    await Task.Delay(delay);
            //}
            return SoftScrollAsync(offset, TimeSpan.FromMilliseconds(offset / 100.0));
        }

        public Task SoftScrollAsync(int offset, TimeSpan duration) {
            return SoftScrollAsync(offset, duration, CancellationToken.None);
        }

        public async Task SoftScrollAsync(int offset, TimeSpan duration, CancellationToken token) {
            if (offset != 0) {
                var shift = 125;
                var sign = offset >= 0 ? 1 : -1;
                var delay = TimeSpan.FromMilliseconds(Math.Abs(shift * (duration.TotalMilliseconds / offset)));
                var script = $"window.scrollBy(0,{sign * shift})";
                var limit = Math.Abs(offset);
                for (var i = 0; i < limit; i += shift) {
                    this.Robot.ExecuteJavaScript(script);
                    await Task.Delay(delay, token);
                }
            }
        }

        public Task WaitContentLoadingAsync() {
            return WaitContentLoadingAsync(this.Robot.PageSearchingTimeout);
        }

        public Task WaitContentLoadingAsync(TimeSpan timeout) {
            return WaitContentLoadingAsync(timeout, CancellationToken.None);
        }

        public Task WaitContentLoadingAsync(TimeSpan timeout, CancellationToken token) {
            return this.Robot.Wait()
                .UsingCancellationToken(token)
                .ThrowTimeoutException()
                .WithTimeout(timeout)
                .UntilAsync(IsContentLoaded);
        }
    }
}
