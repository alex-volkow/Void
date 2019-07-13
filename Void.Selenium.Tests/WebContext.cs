using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.IO;
using Void.Reflection;
using Xunit;

namespace Void.Selenium.Tests
{
    public abstract class WebContext : IDisposable//IAsyncLifetime
    {
        private TempFile template;
        private IWebDriver driver;


        protected IWebDriver GetDriver() {
            if (this.driver == null) {
                var options = new ChromeOptions();
                options.AddArgument("headless");
                this.driver = new ChromeDriver(GetChromedriver().DirectoryName, options);
                var template = typeof(WebContext).Assembly.ReadStringResource("Template.html");
                this.template = new TempFile(Files.CreateTempFile("template.html"));
                File.WriteAllText(this.template.Info.FullName, template);
            }
            return this.driver;
        }

        protected void OpenDefaultPage() {
            var driver = GetDriver();
            var address = new Uri(this.template.Info.FullName);
            driver.Navigate().GoToUrl(address);
        }

        public void Dispose() {
            this.template?.Dispose();
            this.driver?.Dispose();
        }

        private FileInfo GetChromedriver() {
            var directories = new DirectoryInfo[] {
                new DirectoryInfo(Directory.GetCurrentDirectory()),
                Files.Application.Directory
            };
            foreach (var directory in directories) {
                foreach (var file in directory.EnumerateContent()) {
                    if (file.Name.ToLower() == "chromedriver.exe") {
                        return file;
                    }
                }
            }
            throw new FileNotFoundException(
                $"Chromedriver is not found"
                );
        }
    }
}
