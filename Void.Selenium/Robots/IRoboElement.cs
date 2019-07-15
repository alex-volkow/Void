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
        Task<IRoboElement> SetText(string text);
        Task<IRoboElement> SetText(string text, TimeSpan duration);
        Task<IRoboElement> AppendText(string text);
        Task<IRoboElement> AppendText(string text, TimeSpan duration);
        IRoboElement Submit();
        IRoboElement Click();
        IRoboElement Clear();
        IRoboElement MouseOver();
        IRoboElement WithJavaScript();
    }
}
