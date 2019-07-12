﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using Xunit;

namespace Void.Selenium.Tests
{
    public class WebPointerTests : WebContext
    {
        public WebPointerTests() {
            this.Driver.Navigate().GoToUrl("http://icanhazip.com/");
        }

        [Fact]
        public void FindExistingElement() {
            var pointer = CreateWebPointer();
            pointer.Match();
            Assert.True(pointer.Matched);
        }

        [Fact]
        public void FindNonExistingElement() {
            var pointer = CreateWebPointer("//body1");
            pointer.Match();
            Assert.True(!pointer.Matched);
        }

        [Fact]
        public void FindStaledElement() {
            var pointer = CreateWebPointer();
            pointer.Match();
            Assert.True(pointer.Matched);
            this.Driver.Navigate().GoToUrl("http://icanhazip.com/");
            Assert.True(pointer.Staled);
        }

        [Fact]
        public void AutoUpdateElement() {
            var pointer = CreateWebPointer();
            pointer.Match();
            Assert.True(pointer.Matched);
            this.Driver.Navigate().GoToUrl("http://icanhazip.com/");
            Assert.True(pointer.Staled);
            Assert.NotNull(pointer.Required());
        }

        private WebPointer CreateWebPointer(string xpath = "//body") {
            return new WebPointer(this.Driver, By.XPath(xpath));
        }
    }
}
