using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
        IRoboElement UsingElement(string xpath);
        IRoboElement UsingElement(By locator);
        object ExecuteJavaScript(string script);
        Task WaitRandomAsync(Delays delay);
        Task WaitRandomAsync(Delays delay, CancellationToken token);
        Task WaitRandomAsync(TimeSpan delay);
        Task WaitRandomAsync(TimeSpan delay, CancellationToken token);
        Task WaitRandomAsync(int milliseconds);
        Task WaitRandomAsync(int milliseconds, CancellationToken token);
        Task WaitRandomAsync(Delays from, Delays to);
        Task WaitRandomAsync(Delays from, Delays to, CancellationToken token);
        Task WaitRandomAsync(TimeSpan from, TimeSpan to);
        Task WaitRandomAsync(TimeSpan from, TimeSpan to, CancellationToken token);
        Task WaitRandomAsync(int fromMs, int toMs);
        Task WaitRandomAsync(int fromMs, int toMs, CancellationToken token);
    }
}
