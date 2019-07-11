using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Void.Selenium.Tests
{
    public abstract class WebContext : IDisposable//IAsyncLifetime
    {
        protected IWebDriver Driver { get; }



        public WebContext() {
            var options = new ChromeOptions();
            options.AddArgument("headless");
            this.Driver = new ChromeDriver(options);
        }


        public void Dispose() {
            this.Driver.Dispose();
        }
    }
}
