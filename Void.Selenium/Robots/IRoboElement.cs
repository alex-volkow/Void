using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Void.Selenium
{
    public interface IRoboElement : IWrapsElement
    {
        Task<IRoboElement> SendKeysAsync(string text);
        Task<IRoboElement> SendKeysAsync(string text, TimeSpan duration);
        IRoboElement SendKeys(string text);
        IRoboElement Submit();
        IRoboElement Click();
        IRoboElement Clear();
        IRoboElement MoveTo();
        IRoboElement MouseOver();
        IRoboElement WithJavaScript();
        ISelector GetSelector();
    }
}
