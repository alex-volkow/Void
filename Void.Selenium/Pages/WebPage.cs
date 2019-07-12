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
    public class WebPage : IWebPage
    {
        private readonly Lazy<IReadOnlyList<WebPageReflectionElement>> elements;


        public Type Type { get; }

        public object Content { get; }

        public IWebDriver WrappedDriver { get; }

        public bool IsMatched {
            get {
                if (this.Content is IWebMatcher matcher) {
                    if (!matcher.IsMatching(this.WrappedDriver)) {
                        return false;
                    }
                }
                return GetElements()
                    .Where(e => !e.IsOptional)
                    .All(e => e.IsMatched);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public WebPage(IWebDriver driver, Type type) {
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.WrappedDriver = driver ?? throw new ArgumentNullException(nameof(driver));
            this.elements = new Lazy<IReadOnlyList<WebPageReflectionElement>>(ExtractElements);
            this.Content = CreatePage();
        }



        public void Required() {
            var match = Match();
            if (!match.Success) {
                var message = string.Join("; ", match.Errors);
                throw new NotFoundException(message);
            }
        }

        public IWebPageMatch Match() {
            var match = new WebPageMatch();
            if (this.Content is IWebMatcher matcher) {
                if (!matcher.IsMatching(this.WrappedDriver)) {
                    match.Errors.Add(
                        $"Condition '{nameof(IWebMatcher.IsMatching)}' " +
                        $"of '{this.Type.GetNameWithNamespaces()}' is not met"
                        );
                }
            }
            foreach (var element in this.elements.Value) {
                if (!element.IsMatched) {
                    if (element.Match() == null) {
                        if (element.IsFoundButNotVisible) {
                            match.Errors.Add($"Element '{element.Name}' found but not visible");
                        }
                        else {
                            match.Errors.Add($"Element '{element.Name}' is not found");
                        }
                    }
                }
            }
            return match;
        }

        public IEnumerable<IWebPageElement> GetElements() {
            return this.elements.Value;
        }

        protected virtual object CreatePage() {
            if (this.Type.HasDefaultConstructor()) {
                var page = Activator.CreateInstance(this.Type);
                var fields = this.Type.GetTopFields(
                    BindingFlags.Instance | 
                    BindingFlags.NonPublic | 
                    BindingFlags.Public
                    );
                foreach (var field in fields.Where(e => !e.IsInitOnly)) {
                    if (field.FieldType.Is<IWebDriver>() ||
                        field.FieldType == this.WrappedDriver.GetType()) {
                        field.SetValue(page, this.WrappedDriver);
                    }
                }
                return page;
            }
            if (this.Type.HasConstructor(typeof(IWebDriver))) {
                return Activator.CreateInstance(this.Type, this.WrappedDriver);
            }
            throw new InvalidOperationException(
                $"Type must have default constructor or " +
                $"constructor with {nameof(IWebDriver)} parameter"
                );
        }

        private IReadOnlyList<WebPageReflectionElement> ExtractElements() {
            var elements = new List<WebPageReflectionElement>();
            elements.AddRange(ExtractFields());
            elements.AddRange(ExtractElements());
            return elements;
        }

        private IReadOnlyList<WebPageFieldElement> ExtractFields() {
            var members = this.Type
                .GetTopFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(e => e.FieldType == typeof(IWebElement))
                .Where(e => !e.IsInitOnly);
            return GetElementMembers(members)
                .Select(e => new WebPageFieldElement(this.WrappedDriver, e, this.Content))
                .ToArray();
        }

        private IReadOnlyList<WebPagePropertyElement> ExtractProperties() {
            var members = this.Type
                .GetTopProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(e => e.PropertyType == typeof(IWebElement))
                .Where(e => e.CanWrite);
            return GetElementMembers(members)
                .Select(e => new WebPagePropertyElement(this.WrappedDriver, e, this.Content))
                .ToArray();
        }

        private IEnumerable<T> GetElementMembers<T>(IEnumerable<T> members) where T : MemberInfo {
            foreach (var member in members) {
                if (member.GetCustomAttributes<XPathAttribute>().Any()) {
                    yield return member;
                    continue;
                }
                if (member.GetCustomAttributes<FindsByAttribute>().Any()) {
                    yield return member;
                    continue;
                }
            }
        }
    }
}
