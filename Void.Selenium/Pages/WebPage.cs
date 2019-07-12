using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Void.Reflection;

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

        private IReadOnlyList<WebPageFieldElement> ExtractFields() {
            var members = this.Type
                .GetTopFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(e => !e.IsInitOnly);
            return GetElementMembers(members)
                .Select(e => new WebPageFieldElement(e, this.Content))
                .ToArray();
        }

        private IReadOnlyList<WebPagePropertyElement> ExtractProperties() {
            var members = this.Type
                .GetTopProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(e => e.CanWrite);
            return GetElementMembers(members)
                .Select(e => new WebPagePropertyElement(e, this.Content))
                .ToArray();
        }

        private IEnumerable<T> GetElementMembers<T>(IEnumerable<T> members) where T : MemberInfo {
            foreach (var member in members) {
                if (member.GetCustomAttribute<XPathAttribute>() != null) {
                    yield return member;
                    continue;
                }
                if (member.GetCustomAttribute<FindsByAttribute>() != null) {
                    yield return member;
                    continue;
                }
            }
        }
    }
}
