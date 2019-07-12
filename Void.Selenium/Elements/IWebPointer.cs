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
        bool IsMatched { get; }
        bool IsStaled { get; }

        IWebElement Required();
        IWebElement Match();
    }
}
