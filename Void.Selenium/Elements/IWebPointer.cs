using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Selenium
{
    public interface IWebPointer : IWebElement
    {
        ISearchContext Context { get; }
        bool Matched { get; }
        bool Staled { get; }
        By Locator { get; }

        bool Match();
    }
}
