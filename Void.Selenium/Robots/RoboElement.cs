using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Void.Selenium
{
    abstract class RoboElement : RoboComponent, IRoboElement
    {
        public abstract IWebElement WrappedElement { get; }

        public ISearchContext Context { get; }

        public bool UsingJavaScript { get; set; }

        public bool IsMatched => this.WrappedElement != null && !this.IsStaled;

        public bool IsStaled {
            get {
                try {
                    return this.WrappedElement != null
                        ? this.WrappedElement.Displayed && false
                        : true;
                }
                catch (StaleElementReferenceException) {
                    return true;
                }
            }
        }



        public RoboElement(IRobot robot, ISearchContext context) 
            : base(robot) {
            this.Context = context ?? robot.WrappedDriver;
        }



        public abstract IWebElement Match();

        public abstract IWebElement Required();

        public Task<IRoboElement> AppendText(string text) {
            return AppendText(text, GetKeySendingInterval(text), CancellationToken.None);
        }

        public async Task<IRoboElement> AppendText(string text, TimeSpan duration, CancellationToken token) {
            Required().MoveTo();
            if (text?.Length > 0) {
                var time = TimeSpan.FromMilliseconds(duration.TotalMilliseconds / text.Length);
                async Task JavaScriptInput() {
                    Required().ClickJavaScript();
                    foreach (var c in text) {
                        this.Required().SendKeysJavaScript(c.ToString());
                        await Task.Delay(time, token);
                    }
                };
                if (!this.UsingJavaScript && Required().IsVisible()) {
                    try {
                        Required().Click();
                        foreach (var c in text) {
                            this.Required().SendKeys(c.ToString());
                            await Task.Delay(time, token);
                        }
                    }
                    catch {
                        await JavaScriptInput();
                    }
                }
                else {
                    await JavaScriptInput();
                }
            }
            return this;
        }

        public Task<IRoboElement> SetText(string text) {
            return SetText(text, GetKeySendingInterval(text), CancellationToken.None);
        }

        public Task<IRoboElement> SetText(string text, TimeSpan duration, CancellationToken token) {
            Clear();
            return AppendText(text, duration, token);
        }

        public IRoboElement Clear() {
            Required().MoveTo();
            try {
                if (!this.UsingJavaScript || Required().IsVisible()) {
                    Required().Clear();
                }
                else {
                    Required().ClearJavaScript();
                }
            }
            catch {
                Required().ClearJavaScript();
            }
            return this;
        }

        public IRoboElement Click() {
            Required().MoveTo();
            try {
                if (!this.UsingJavaScript || Required().IsVisible()) {
                    Required().Click();
                }
                else {
                    Required().ClickJavaScript();
                }
            }
            catch {
                Required().ClickJavaScript();
            }
            return this;
        }

        public IRoboElement MouseOver() {
            Required().MoveTo();
            Required().MouseOver();
            return this;
        }

        public IRoboElement Submit() {
            Required().MoveTo();
            try {
                if (!this.UsingJavaScript || Required().IsVisible()) {
                    Required().Submit();
                }
                else {
                    Required().SubmitJavaScript();
                }
            }
            catch {
                Required().SubmitJavaScript();
            }
            return this;
        }

        public IRoboElement WithJavaScript() {
            this.UsingJavaScript = true;
            return this;
        }

        private TimeSpan GetKeySendingInterval(string text) {
            if (!string.IsNullOrEmpty(text)) {
                //var deviation = RANDOM.NextDouble() > 0.5 ? 0.5 : -0.5;
                //deviation = (1 + deviation * RANDOM.NextDouble());
                var milliseconds = text.Length * this.Robot.KeySendingInterval.TotalMilliseconds;
                return TimeSpan.FromMilliseconds(milliseconds);
            }
            return TimeSpan.Zero;
        }
    }
}
