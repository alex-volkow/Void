using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Selenium
{
    public interface IWebMatcher
    {
        bool IsMatching(IWebDriver driver);
    }
}
