using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Selenium
{
    public interface IWebPage<out T> : IWebPage where T : class
    {
        new T Content { get; }
    }

    public interface IWebPage : IWrapsDriver
    {
        Type Type { get; }
        bool IsMatched { get; }
        object Content { get; }

        void Required();
        IWebPageMatch Match();
        IEnumerable<IWebPageElement> GetElements();
    }
}
