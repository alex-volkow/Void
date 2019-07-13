using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Void.Selenium
{
    public interface IRobot : IWrapsDriver
    {
        TimeSpan KeySendingInterval { get; }
        TimeSpan PageSearchingTimeout { get; }
        TimeSpan ElementSearchingTimeout { get; }
        TimeSpan ConditionWaitingTimeout { get; }
        TimeSpan ConditionCheckingInterval { get; }
        double RandomWaitDeviationPercent { get; }

        IRoboElements Elements { get; }
        IRoboBrowser Browser { get; }
        IRoboPages Pages { get; }

        IRoboWait Wait();
        IRoboElement Using(IWebElement element);
        IRoboElement Using(IWebPointer pointer);
        IRoboElement Using(string xpath);
        IRoboElement Using(By locator);
        object ExecuteJavaScript(string script);
        Task WaitRandomAsync(Delays delay);
        Task WaitRandomAsync(TimeSpan delay);
        Task WaitRandomAsync(int milliseconds);
        Task WaitRandomAsync(Delays from, Delays to);
        Task WaitRandomAsync(TimeSpan from, TimeSpan to);
        Task WaitRandomAsync(int fromMs, int toMs);
        Task WaitContentLoadingAsync();
        Task WaitContentLoadingAsync(TimeSpan timeout);
    }
}
