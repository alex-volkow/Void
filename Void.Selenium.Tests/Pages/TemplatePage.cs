using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Selenium.Tests
{
    public class TemplatePage
    {
        [XPath("//body")]
        public IWebElement Body { get; }

        [FindsBy(How = How.Id, Using = "username")]
        public IWebElement Username { get; }
    }
}
