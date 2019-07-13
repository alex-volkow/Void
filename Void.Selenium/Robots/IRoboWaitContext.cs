using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Void.Selenium
{
    public interface IRoboWaitContext
    {
        int Iteration { get; }
        TimeSpan TimeLeft { get; }
        TimeSpan Elapsed { get; }
        TimeSpan Timeout { get; }
        TimeSpan Interval { get; }
        bool IsThrowTimeoutException { get; }
        bool IsIgnoreConditionExceptions { get; }
        Func<Exception, bool> ExceptionHandler { get; }
        CancellationToken CancellationToken { get; }

        void Break();
        void Throw<T>() where T : Exception, new();
        void Throw<T>(string message) where T : Exception;
    }
}
