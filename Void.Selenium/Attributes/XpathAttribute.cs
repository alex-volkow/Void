using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Selenium
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class XPathAttribute : Attribute
    {
        public string XPath { get; set; }

        public int Priority { get; set; }

        public Type CustomFinderType { get; set; }



        public XPathAttribute() { }

        public XPathAttribute(string xpath) {
            this.XPath = xpath;
        }
    }
}
