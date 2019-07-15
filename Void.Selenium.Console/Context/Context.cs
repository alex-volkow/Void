using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Void.IO;
using Void.Diagnostics;
using Void.Reflection;
using Void.Selenium.Console.Properties;

namespace Void.Selenium.Console
{
    class Context : ISelectDriverContext
    {
        private readonly MainWindow window;
        private IWebDriver driver;


        public FileInfo Chromedriver { get; private set; }

        public FileInfo Gekodriver { get; private set; }

        public string Status {
            get => (string)this.window.status.Content;
            set => this.window.status.Content = value;
        }


        public Context(MainWindow window) {
            this.window = window ?? throw new ArgumentNullException(nameof(window));
            this.window.Closing += Closing;
        }



        public FileInfo GetChromedriver() {
            return Files.Application.Directory
                .GetContent()
                .FirstOrDefault(e => e.Name.ToLower() == "chromedriver.exe");
        }

        public FileInfo GetGekodriver() {
            return Files.Application.Directory
                .GetContent()
                .FirstOrDefault(e => e.Name.ToLower() == "geckodriver.exe");
        }

        public FileInfo GetTorExecutable() {
            throw new NotImplementedException();
        }

        public async void StartChrome() {
            //var progress = new ProgressWindow {
            //    Header = "Initializing browser",
            //    Message = "Loading Google Chrome..."
            //};
            //await progress.ShowProgress(() => {
            //    this.driver = new ChromeDriver();
            //});
            //OpenPage<BrowserPage>();
            //Process.GetCurrentProcess().FocusWindow();
            this.Status = "Initializing driver";
            this.window.frame.IsEnabled = false;
            await Task.Run(() => this.driver = new ChromeDriver());
            this.Status = "Driver initialized";
            this.window.frame.IsEnabled = true;
            OpenPage<BrowserPage>();
            Process.GetCurrentProcess().FocusWindow();
        }

        public async void StartFirefox() {
            //var progress = new ProgressWindow {
            //    Header = "Initializing browser",
            //    Message = "Loading Firefox..."
            //};
            //await progress.ShowProgress(() => {
            //    this.driver = new FirefoxDriver();
            //});
            this.Status = "Initializing driver";
            this.window.frame.IsEnabled = false;
            await Task.Run(() => this.driver = new FirefoxDriver());
            this.Status = "Driver initialized";
            this.window.frame.IsEnabled = true;
            OpenPage<BrowserPage>();
            Process.GetCurrentProcess().FocusWindow();
        }

        public void OpenPage<T>() {
            OpenPage(typeof(T));
        }

        public void OpenPage(Type type) {
            if (type == null) {
                throw new ArgumentNullException(nameof(type));
            }
            if (type.HasDefaultConstructor()) {
                this.window.frame.Content = Activator.CreateInstance(type);
            }
            else {
                var constructor = type.GetConstructors()
                    .Where(e => e.GetParameters().Count() == 1)
                    .FirstOrDefault(e => GetType().Is(e.GetParameters().First().ParameterType));
                if (constructor == null) {
                    throw new InvalidOperationException(
                        $"Failed to create instance of {type.GetNameWithNamespaces()}"
                        );
                }
                this.window.frame.Content = constructor.Invoke(new object[] { this });
            }
        }

        public Task Initialize() {
            this.Chromedriver = GetChromedriver();
            this.Gekodriver = GetGekodriver();
            return Task.CompletedTask;
        }

        public async void Closing(object sender, CancelEventArgs e) {
            Settings.Default.MainWindowHeight = this.window.Height;
            Settings.Default.MainWindowWidth = this.window.Width;
            Settings.Default.Save();
            if (this.driver != null) {
                var progress = new ProgressWindow {
                    Header = "Closing browser",
                    Message = "Disposing browser resources..."
                };
                await progress.ShowProgress((Action)this.driver.Dispose);
            }
        }
    }
}
