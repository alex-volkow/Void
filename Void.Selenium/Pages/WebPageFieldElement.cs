using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OpenQA.Selenium;

namespace Void.Selenium
{
    internal class WebPageFieldElement : WebPageReflectionElement
    {
        public WebPageFieldElement(ISearchContext context, FieldInfo member, object page) 
            : base(context, member, page) {
        }

        protected override IWebElement GetMemberValue() {
            return (IWebElement)((FieldInfo)member).GetValue(this.Page);
        }

        protected override void SetMemberValue(IWebElement element) {
            ((FieldInfo)member).SetValue(this.Page, element);
        }
    }
}
