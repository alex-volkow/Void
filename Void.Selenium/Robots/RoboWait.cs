using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Void.Selenium
{
    class RoboWait : RoboComponent, IRoboWait
    {
        public TimeSpan Timeout { get; private set; }
        public TimeSpan Interval { get; private set; }
        public bool IsThrowTimeoutException { get; private set; }
        public bool IsIgnoreConditionExceptions { get; private set; }
        public Func<Exception, bool> ExceptionHandler { get; private set; }
        public CancellationToken CancellationToken { get; private set; }


        public RoboWait(IRobot robot) 
            : base(robot) {
        }


        public Task UntilAsync(Func<bool> condition) {
            if (condition == null) {
                throw new ArgumentNullException(nameof(condition));
            }
            return UntilAsync(context => condition());
        }

        public Task UntilAsync(Func<IRoboWaitContext, bool> condition) {
            if (condition == null) {
                throw new ArgumentNullException(nameof(condition));
            }
            return UntilAsync(context => condition(context));
        }

        public Task UntilAsync(Func<object> condition) {
            if (condition == null) {
                throw new ArgumentNullException(nameof(condition));
            }
            return UntilAsync(context => condition());
        }

        public async Task UntilAsync(Func<IRoboWaitContext, object> condition) {
            if (condition == null) {
                throw new ArgumentNullException(nameof(condition));
            }
            var context = CreateContext();
            while (context.IsActive) {
                context.Iteration++;
                try {
                    if (context.CancellationToken.IsCancellationRequested) {
                        throw new OperationCanceledException();
                    }
                    try {
                        var result = condition(context);
                        if (result != null) {
                            if (result is bool? || result is bool) {
                                if (object.Equals(result, true)) {
                                    return;
                                }
                            }
                            else {
                                return;
                            }
                        }
                    }
                    catch (Exception ex) {
                        if (!context.IsIgnoreConditionExceptions) {
                            if (!(context.ExceptionHandler?.Invoke(ex) ?? false)) {
                                throw;
                            }
                        }
                    }
                    await Task.Delay(context.Interval, context.CancellationToken);
                }
                catch (Exception ex) {
                    if (context.ExceptionHandler?.Invoke(ex) ?? false) {
                        return;
                    }
                    else {
                        throw;
                    }
                }
            }
            if (context.IsThrowTimeoutException) {
                var timeout = new TimeoutException();
                if (!(context.ExceptionHandler?.Invoke(timeout) ?? false)) {
                    throw timeout;
                }
            }
        }

        public Task UntilAsync<T>(Func<T> condition) where T : class {
            if (condition == null) {
                throw new ArgumentNullException(nameof(condition));
            }
            return UntilAsync(context => (T)condition());
        }

        public Task UntilAsync<T>(Func<IRoboWaitContext, T> condition) where T : class {
            if (condition == null) {
                throw new ArgumentNullException(nameof(condition));
            }
            return UntilAsync(context => (T)condition(context));
        }

        public IRoboWait IgnoreConditionExceptions() {
            this.IsIgnoreConditionExceptions = true;
            return this;
        }

        public IRoboWait NotIgnoreConditionExceptions() {
            this.IsIgnoreConditionExceptions = false;
            return this;
        }

        public IRoboWait ThrowTimeoutException() {
            this.IsThrowTimeoutException = true;
            return this;
        }

        public IRoboWait NotThrowTimeoutException() {
            this.IsThrowTimeoutException = false;
            return this;
        }

        public IRoboWait UsingCancellationToken(CancellationToken token) {
            this.CancellationToken = token;
            return this;
        }

        public IRoboWait RemoveCancellationToken() {
            this.CancellationToken = CancellationToken.None;
            return this;
        }

        public IRoboWait UsingExceptionHandler(Func<Exception, bool> handler) {
            this.ExceptionHandler = handler;
            return this;
        }

        public IRoboWait RemoveExceptionHandler() {
            this.ExceptionHandler = null;
            return this;
        }

        public IRoboWait WithInterval(TimeSpan value) {
            this.Interval = value;
            return this;
        }

        public IRoboWait WithTimeout(TimeSpan value) {
            this.Timeout = value;
            return this;
        }

        private RoboWaitContext CreateContext() {
            return new RoboWaitContext {
                IsIgnoreConditionExceptions = this.IsIgnoreConditionExceptions,
                IsThrowTimeoutException = this.IsThrowTimeoutException,
                Limit = DateTime.Now + this.Timeout.Duration(),
                CancellationToken = this.CancellationToken,
                ExceptionHandler = this.ExceptionHandler,
                Interval = this.Interval,
                Timeout = this.Timeout
            };
        }
    }
}
