using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Void.Selenium.Console
{
    public partial class SelectDriverPage : Page
    {
        private readonly IReadOnlyCollection<Button> buttons;
        private readonly ISelectDriverContext context;


        public SelectDriverPage(ISelectDriverContext context) {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            InitializeComponent();
            this.buttons = new Button[] {
                this.continueButton,
                this.chromeButton,
                this.firefoxButton,
                this.torButton
            };
        }


        private async void PageLoaded(object sender, RoutedEventArgs e) {
            var progress = new ProgressWindow {
                Header = "Loading drivers",
                Message = "Wait while drivers will be loaded..."
            };
            foreach (var button in this.buttons) {
                button.Visibility = Visibility.Collapsed;
            }
            this.continueLabel.Visibility = Visibility.Collapsed;
            var result = await progress.ShowProgress(LoadDrivers);
            this.chromeButton.Visibility = result.Chrome != null ? Visibility.Visible : Visibility.Hidden;
            this.firefoxButton.Visibility = result.Firefox != null ? Visibility.Visible : Visibility.Hidden;
            //this.torButton.Visibility = result.Firefox != null ? Visibility.Visible : Visibility.Hidden;
            if (result.Chrome == null && result.Firefox == null) {
                this.driversInfo.Content = "No drivers found. Add any driver to root application folder.";
            }
        }

        private void ChromeButton_Click(object sender, RoutedEventArgs e) {

        }

        private void FirefoxButton_Click(object sender, RoutedEventArgs e) {

        }

        private void TorButton_Click(object sender, RoutedEventArgs e) {

        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e) {

        }

        private Task<DriverLoadingResult> LoadDrivers() {
            var result = new DriverLoadingResult {
                Chrome = this.context.GetChromedriver(),
                Firefox = this.context.GetGekodriver()
            };
            //if (result.Firefox != null) {
            //    InitializeTor();
            //}
            return Task.FromResult(result);
        }

        private void InitializeTor() {
            Thread.Sleep(3000);
        }

        private class DriverLoadingResult
        {
            public FileInfo Chrome { get; set; }
            public FileInfo Firefox { get; set; }
            public FileInfo Tor { get; set; }
        }
    }
}
