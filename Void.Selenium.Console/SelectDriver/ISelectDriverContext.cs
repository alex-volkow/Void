using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Selenium.Console
{
    public interface ISelectDriverContext : IStatusContext
    {
        FileInfo Chromedriver { get; }
        FileInfo Gekodriver { get; }

        void StartChrome();
        void StartFirefox();
    }
}
