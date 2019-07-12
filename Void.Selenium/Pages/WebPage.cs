using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Void.Selenium
{
    public abstract class WebPage : IWebPage
    {
        public Type Type => throw new NotImplementedException();

        public bool IsMatched => throw new NotImplementedException();

        public object Content { get; }

        public IWebDriver WrappedDriver { get; }



        public WebPage(IWebDriver driver, object content) {
            this.Content = content ?? throw new ArgumentNullException(nameof(content));
            this.WrappedDriver = driver ?? throw new ArgumentNullException(nameof(driver));

            //FindsByXpathAttribute;
            //FindsByAttribute;
            //FindsByAllAttribute;
            //FindsBySequenceAttribute;
            //CacheLookupAttribute;
            //VisibleAttribute;
            //OptionalAttribute
        }



        public IEnumerable<IWebElement> GetElements() {
            throw new NotImplementedException();
        }

        public IWebPageMatch Match() {
            throw new NotImplementedException();
        }
    }
}
