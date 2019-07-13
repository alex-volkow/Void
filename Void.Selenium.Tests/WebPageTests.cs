using System;
using System.Collections.Generic;
using System.Linq;
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
