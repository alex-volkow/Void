using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.Reflection;

namespace Void.Selenium.Console
{
    class Context : ISelectDriverContext
    {
        private readonly MainWindow window;


        public Context(MainWindow window) {
            this.window = window ?? throw new ArgumentNullException(nameof(window));
        }


        public FileInfo GetChromedriver() {
            throw new NotImplementedException();
        }

        public FileInfo GetGekodriver() {
            throw new NotImplementedException();
        }

        public FileInfo GetTorExecutable() {
            throw new NotImplementedException();
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
    }
}
