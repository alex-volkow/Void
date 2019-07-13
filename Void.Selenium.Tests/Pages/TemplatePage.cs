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
        public IWebElement body;

        [XPath("//input[@id='username1']")]
        [FindsBy(How = How.Id, Using = "username")]
        public IWebElement username1;

        [Visible]
        [FindsByAll]
        [XPath("//input[@id='username']")]
        [FindsBy(How = How.Name, Using = "username")]
        [XPath("//input[@id='username' and @name='username']")]
        private IWebElement username2;

        [FindsBySequence]
        [XPath("//form[@id='root-form']")]
        [XPath("//input[@id='password']")]
        protected IWebElement password1;

        [Optional]
        [FindsBySequence]
        [XPath("//input[@id='password']")]
        [XPath("//form[@id='root-form']")]
        private IWebElement password2;

        [FindsBySequence]
        [XPath("//input[@id='password']", Priority = 1)]
        [XPath("//form[@id='root-form']", Priority = 0)]
        private IWebElement password3;

        [FindsBySequence]
        [XPath("//input[@id='password']", Priority = 1)]
        [XPath("//form[@id='root-form']", Priority = 0)]
        private readonly IWebElement password4;



        [XPath("//body")]
        public IWebElement Body { get; }

        [XPath("//input[@id='username1']")]
        [FindsBy(How = How.Id, Using = "username")]
        public IWebElement Username1 { get; }

        [Visible]
        [FindsByAll]
        [XPath("//input[@id='username']")]
        [FindsBy(How = How.Name, Using = "username")]
        [XPath("//input[@id='username' and @name='username']")]
        public IWebElement Username2 { get; }

        [FindsBySequence]
        [XPath("//form[@id='root-form']")]
        [XPath("//input[@id='password']")]
        public IWebElement Password1 { get; }

        [Optional]
        [FindsBySequence]
        [XPath("//input[@id='password']")]
        [XPath("//form[@id='root-form']")]
        public IWebElement Password2 { get; }

        [FindsBySequence]
        [XPath("//input[@id='password']", Priority = 1)]
        [XPath("//form[@id='root-form']", Priority = 0)]
        public IWebElement Password3 { get; }
    }
}
