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


        private void PageLoaded(object sender, RoutedEventArgs e) {
            //var progress = new ProgressWindow {
            //    Header = "Loading drivers",
            //    Message = "Wait while drivers will be loaded..."
            //};
            foreach (var button in this.buttons) {
                button.Visibility = Visibility.Collapsed;
            }
            this.continueLabel.Visibility = Visibility.Collapsed;
            this.chromeButton.Visibility = this.context.Chromedriver != null ? Visibility.Visible : Visibility.Hidden;
            this.firefoxButton.Visibility = this.context.Gekodriver != null ? Visibility.Visible : Visibility.Hidden;
            //this.torButton.Visibility = result.Firefox != null ? Visibility.Visible : Visibility.Hidden;
            if (this.context.Chromedriver == null && this.context.Gekodriver == null) {
                this.driversInfo.Content = "No drivers found. Add any driver to root application folder.";
            }
        }

        private void ChromeButton_Click(object sender, RoutedEventArgs e) {
            this.context.StartChrome();
        }

        private void FirefoxButton_Click(object sender, RoutedEventArgs e) {
            this.context.StartFirefox();
        }

        private void TorButton_Click(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }
    }
}
