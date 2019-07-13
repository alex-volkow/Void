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
        Task SendKeysAsync(string text);
        Task SendKeysAsync(string text, TimeSpan duration);
        void SendKeys(string text);
        void Submit();
        IRoboElement Click();
        IRoboElement Clear();
        IRoboElement MoveTo();
        IRoboElement MouseOver();
        IRoboElement WithJavaScript();
        ISelector GetSelector();
    }
}
