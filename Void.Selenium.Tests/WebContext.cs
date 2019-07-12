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
        private readonly TempFile template;


        protected IWebDriver Driver { get; }



        public WebContext() {
            var options = new ChromeOptions();
            options.AddArgument("headless");
            this.Driver = new ChromeDriver(GetChromedriver().DirectoryName, options);
            var template = typeof(WebContext).Assembly.ReadStringResource("Template.html");
            this.template = new TempFile();
            File.WriteAllText(this.template.Info.FullName, template); 
        }


        protected void OpenDefaultPage() {
            var address = new Uri(this.template.Info.FullName);
            this.Driver.Navigate().GoToUrl(address);
        }

        public void Dispose() {
            this.template.Dispose();
            this.Driver.Dispose();
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
