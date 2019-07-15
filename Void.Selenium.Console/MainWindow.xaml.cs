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

namespace Void.Selenium.Console
{

    public partial class MainWindow : Window
    {
        private readonly Context context;


        public MainWindow() {
            InitializeComponent();
            this.context = new Context(this);
        }

        private void WindowLoaded(object sender, RoutedEventArgs e) {
            this.context.OpenPage<SelectDriverPage>();
        }
    }
}
