using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OpenQA.Selenium;
using Void.Reflection;

namespace Void.Selenium
{
    internal class WebPagePropertyElement : WebPageReflectionElement
    {
        public WebPagePropertyElement(ISearchContext context, PropertyInfo member, object page) 
            : base(context, member, page) {
        }

        protected override IWebElement GetMemberValue() {
            return (IWebElement)((PropertyInfo)this.member).GetValue(this.Page);
        }

        protected override void SetMemberValue(IWebElement element) {
            ((PropertyInfo)member).SetForce(this.Page, element);
        }
    }
}
