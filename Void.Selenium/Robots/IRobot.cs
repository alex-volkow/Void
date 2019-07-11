using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Void.Selenium
{
    public interface IRobot : IWrapsDriver
    {
        bool IsContentLoaded();
        Task WaitContentLoadingAsync();
        Task WaitContentLoadingAsync(TimeSpan timeout);
        byte[] GetScreenshot();
    }
}
