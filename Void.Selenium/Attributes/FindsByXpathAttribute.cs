using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Selenium
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class FindsByXpathAttribute : Attribute, IComparable
    {
        public string XPath { get; set; }

        public int Priority { get; set; }



        public FindsByXpathAttribute() { }

        public FindsByXpathAttribute(string xpath) {
            this.XPath = xpath;
        }



        public int CompareTo(object obj) {
            throw new NotImplementedException();
        }
    }
}
