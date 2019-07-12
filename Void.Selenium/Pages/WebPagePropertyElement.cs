using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OpenQA.Selenium;

namespace Void.Selenium
{
    internal class WebPagePropertyElement : IWebPageElement
    {
        public bool IsOptional => throw new NotImplementedException();

        public bool IsVisible => throw new NotImplementedException();

        public ISearchContext Context => throw new NotImplementedException();

        public bool IsMatched => throw new NotImplementedException();

        public bool IsStaled => throw new NotImplementedException();

        public IWebElement WrappedElement => throw new NotImplementedException();


        public WebPagePropertyElement(PropertyInfo member, object content) {

        }


        public IWebElement Match() {
            throw new NotImplementedException();
        }

        public IWebElement Required() {
            throw new NotImplementedException();
        }
    }
}
