using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Selenium.Console
{
    public interface ISelectDriverContext
    {
        FileInfo GetChromedriver();
        FileInfo GetGekodriver();
        FileInfo GetTorExecutable();
    }
}
