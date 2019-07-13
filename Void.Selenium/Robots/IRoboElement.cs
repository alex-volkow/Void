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
        Task SendKeysJavaScriptAsync(string text);
        Task SendKeysJavaScriptAsync(string text, TimeSpan duration);
        void SendKeys(string text);
        void SendKeysJavaScript(string text);
        void Submit();
        void SubmitJavaScript();
        IRoboElement Click();
        IRoboElement ClickJavaScript();
        IRoboElement Clear();
        IRoboElement ClearJavaScript();
        IRoboElement MoveTo();
        IRoboElement MouseOver();
        ISelector GetSelector();
    }
}
