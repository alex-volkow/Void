using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Void.Selenium
{
    class RoboWaitContext : IRoboWaitContext
    {
        public bool IsActive => this.Limit > DateTime.Now;

        public int Iteration { get; set; }
        public DateTime Limit { get; set; }
        public TimeSpan TimeLeft { get; set; }
        public TimeSpan Elapsed { get; set; }
        public TimeSpan Timeout { get; set; }
        public TimeSpan Interval { get; set; }
        public bool IsThrowTimeoutException { get; set; }
        public bool IsIgnoreConditionExceptions { get; set; }
        public Func<Exception, bool> ExceptionHandler { get; set; }
        public CancellationToken CancellationToken { get; set; }

        public void Break() {
            throw new NotImplementedException();
        }

        public void Throw<T>() where T : Exception, new() {
            throw new NotImplementedException();
        }

        public void Throw<T>(string message) where T : Exception {
            throw new NotImplementedException();
        }
    }
}
