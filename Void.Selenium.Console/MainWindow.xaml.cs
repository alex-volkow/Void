using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Void.Selenium.Console.Properties;

namespace Void.Selenium.Console
{

    public partial class MainWindow : Window
    {
        private readonly Context context;


        public MainWindow() {
            this.Dispatcher.UnhandledException += UnhandledExceptionRaised;
            InitializeComponent();
            this.context = new Context(this);
            if (Settings.Default.MainWindowHeight > 100 && Settings.Default.MainWindowWidth > 100) {
                this.Height = Settings.Default.MainWindowHeight;
                this.Width = Settings.Default.MainWindowWidth;
            }
        }

        private void UnhandledExceptionRaised(object sender, DispatcherUnhandledExceptionEventArgs e) {
            e.Handled = true;
            ErrorWindow.Show(e.Exception);
        }

        private async void WindowLoaded(object sender, RoutedEventArgs e) {
            //var windows = new ProgressWindow {
            //    Header = "Initializing",
            //    Message = "Initializing application..."
            //};
            //await windows.ShowProgress(this.context.Initialize);
            await this.context.Initialize();
            this.context.OpenPage<SelectDriverPage>();
        }
    }
}
