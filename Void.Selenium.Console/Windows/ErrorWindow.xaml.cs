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
using System.Windows.Shapes;

namespace Void.Selenium.Console
{
    public partial class ErrorWindow : Window
    {
        public ErrorWindow() {
            InitializeComponent();
        }


        public static void Handle(Action action) {
            try {
                action();
            }
            catch (Exception ex) {
                var window = new ErrorWindow();
                window.message.Text = ex.ToString();
                window.ShowDialog();
            }
        }

        public static async Task Handle(Task task) {
            try {
                await task;
            }
            catch (Exception ex) {
                var window = new ErrorWindow();
                window.message.Text = ex.ToString();
                window.ShowDialog();
            }
        }

        public static async Task<T> Handle<T>(Task<T> task) {
            try {
                return await task;
            }
            catch (Exception ex) {
                var window = new ErrorWindow();
                window.message.Text = ex.ToString();
                window.ShowDialog();
                return default(T);
            }
        }

        private void CopyMessageButton_Click(object sender, RoutedEventArgs e) {
            Clipboard.SetText(this.message.Text, TextDataFormat.Text);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
