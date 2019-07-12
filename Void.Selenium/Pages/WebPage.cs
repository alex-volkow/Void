using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace Void.Selenium
{
    public abstract class WebPage : IWebPage
    {
        private readonly Lazy<IReadOnlyList<IWebPageElement>> elements;


        public Type Type => this.Content.GetType();

        public object Content { get; }

        public IWebDriver WrappedDriver { get; }

        public bool IsMatched => throw new NotImplementedException();



        public WebPage(IWebDriver driver, object content) {
            this.Content = content ?? throw new ArgumentNullException(nameof(content));
            this.WrappedDriver = driver ?? throw new ArgumentNullException(nameof(driver));
            this.elements = new Lazy<IReadOnlyList<IWebPageElement>>(ExtractElements);
            //FindsByXpathAttribute;
            //FindsByAttribute;
            //FindsByAllAttribute;
            //FindsBySequenceAttribute;
            //CacheLookupAttribute;
            //VisibleAttribute;
            //OptionalAttribute
        }



        public void Required() {
            throw new NotImplementedException();
            //if (!Match().Success) {
            //    throw new 
            //}
        }

        public IEnumerable<IWebPageElement> GetElements() {
            return this.elements.Value;
        }

        public IWebPageMatch Match() {
            throw new NotImplementedException();
        }

        private IReadOnlyList<IWebPageElement> ExtractElements() {
            throw new NotImplementedException();
        }

        private IEnumerable<FieldInfo> ExtractFields() {
            // except auto-fields
            throw new NotImplementedException();
        }

        private IEnumerable<PropertyInfo> ExtractProperties() {
            throw new NotImplementedException();
        }
    }
}
