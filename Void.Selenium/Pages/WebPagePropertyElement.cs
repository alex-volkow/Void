using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OpenQA.Selenium;

namespace Void.Selenium
{
    internal class WebPagePropertyElement : WebPageReflectionElement
    {
        public WebPagePropertyElement(ISearchContext context, PropertyInfo member, object page) 
            : base(context, member, page) {
        }

        protected override IWebElement GetMemberValue() {
            return (IWebElement)((PropertyInfo)member).GetValue(this.Page);
        }

        protected override void SetMemberValue(IWebElement element) {
            ((PropertyInfo)member).SetValue(this.Page, element);
        }
    }
}
