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
    public partial class ProgressWindow : Window
    {
        private readonly object locker;
        //private bool closing;
        private bool opened;


        public string Header {
            get => (string)this.header.Content;
            set => this.header.Content = value;
        }

        public string Message {
            get => (string)this.message.Content;
            set => this.message.Content = value;
        }

        protected bool IsOpened {
            get {
                lock (this.locker) {
                    return this.opened;
                }
            }
        }



        public ProgressWindow() {
            InitializeComponent();
            this.locker = new object();
            this.Loaded += ProgressWindowLoaded;
        }



        public async Task ShowProgress(Func<Task> action) {
            var task = Handle(action());
            ShowDialog();
            await task;
        }

        public async Task<T> ShowProgress<T>(Func<Task<T>> action) {
            var task = Handle(action());
            //var preventOpening = false;
            //lock (this.locker) {
            //    preventOpening = closing;
            //}
            //if (!preventOpening) {
            //    ShowDialog();
            //}
            ShowDialog();
            return await task;
        }

        private async Task Handle(Task task) {
            try {
                await ErrorWindow.Handle(task);
            }
            finally {
                while (!this.IsOpened) {
                    await Task.Delay(100);
                }
                Close();
            }
        }

        private async Task<T> Handle<T>(Task<T> task) {
            var result = await ErrorWindow.Handle(task);
            while (!this.IsOpened) {
                await Task.Delay(100);
            }
            Close();
            //lock (this.locker) {
            //    if (this.opened) {
            //        Close();
            //    }
            //    else {
            //        closing = true;
            //    }
            //}
            return result;
        }

        private void ProgressWindowLoaded(object sender, RoutedEventArgs e) {
            lock (this.locker) {
                this.Loaded -= ProgressWindowLoaded;
                this.opened = true;
                //if (this.closing) {
                //    Close();
                //}
            }
        }
    }
}
