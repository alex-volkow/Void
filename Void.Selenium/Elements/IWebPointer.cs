using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Selenium
{
    public interface IWebPointer : IWrapsElement
    {
        ISearchContext Context { get; }
        bool Matched { get; }
        bool Staled { get; }
        By Locator { get; }

        IWebElement Required();
        IWebElement Match();
    }
}
