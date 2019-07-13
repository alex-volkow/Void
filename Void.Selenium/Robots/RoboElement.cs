using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Void.Selenium
{
    class RoboElement : RoboComponent, IRoboElement
    {
        public RoboElement(IRobot robot) 
            : base(robot) {
        }

        public IWebElement WrappedElement => throw new NotImplementedException();

        public IRoboElement Clear() {
            throw new NotImplementedException();
        }

        public IRoboElement Click() {
            throw new NotImplementedException();
        }

        public ISelector GetSelector() {
            throw new NotImplementedException();
        }

        public IRoboElement MouseOver() {
            throw new NotImplementedException();
        }

        public IRoboElement MoveTo() {
            throw new NotImplementedException();
        }

        public IRoboElement SendKeys(string text) {
            throw new NotImplementedException();
        }

        public Task<IRoboElement> SendKeysAsync(string text) {
            throw new NotImplementedException();
        }

        public Task<IRoboElement> SendKeysAsync(string text, TimeSpan duration) {
            throw new NotImplementedException();
        }

        public IRoboElement Submit() {
            throw new NotImplementedException();
        }

        public IRoboElement WithJavaScript() {
            throw new NotImplementedException();
        }
    }
}
