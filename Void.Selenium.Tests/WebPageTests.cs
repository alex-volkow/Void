using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Void.Reflection;
using Xunit;

namespace Void.Selenium.Tests
{
    public class WebPageTests : WebContext
    {
        [Fact]
        public void CheckElements() {
            OpenDefaultPage();
            var page = new WebPage<TemplatePage>(GetDriver());
            var elements = page.GetElements();
            foreach (var property in page.Type.GetTopProperties()) {
                Assert.True(elements.Any(e => e.Name == property.Name), $"Not found: {property.Name}");
            }
            var fields = page.Type
                .GetTopFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(e => !e.IsInitOnly);
            foreach (var field in fields) {
                Assert.True(elements.Any(e => e.Name == field.Name), $"Not found: {field.Name}");
            }
        }

        [Fact]
        public void GetElements() {
            OpenDefaultPage();
            var page = new WebPage<TemplatePage>(GetDriver());
            Assert.NotNull(page.GetElement(nameof(TemplatePage.Body)));
            Assert.NotNull(page.GetElement(e => e.Password1));
            Assert.Null(page.GetElement("pew pew"));
        }

        [Fact]
        public void MatchTemplatePage() {
            OpenDefaultPage();
            var page = new WebPage<TemplatePage>(GetDriver());
            Assert.False(page.IsMatched);
            page.Required();
        }
    }
}
