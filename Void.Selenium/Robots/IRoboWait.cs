﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Void.Selenium
{
    public interface IRoboWait
    {
        TimeSpan Timeout { get; }
        TimeSpan Interval { get; }
        bool IsThrowTimeoutException { get; }
        bool IsIgnoreConditionExceptions { get; }
        Func<Exception, bool> ExceptionHandler { get; }
        CancellationToken CancellationToken { get; }

        Task UntilAsync(Func<bool> condition);
        Task UntilAsync(Func<IRoboWaitContext, bool> condition);
        IRoboWait WithTimeout(TimeSpan value);
        IRoboWait WithInterval(TimeSpan value);
        IRoboWait ThrowTimeoutException();
        IRoboWait NotThrowTimeoutException();
        IRoboWait IgnoreConditionExceptions();
        IRoboWait NotIgnoreConditionExceptions();
        IRoboWait UsingExceptionHandler(Func<Exception, bool> handler);
        IRoboWait RemoveExceptionHandler();
        IRoboWait UsingCancellationToken(CancellationToken token);
        IRoboWait RemoveCancellationToken();
    }
}
