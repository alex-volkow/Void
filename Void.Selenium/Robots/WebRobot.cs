using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Void.Selenium
{
    public class WebRobot : IRobot
    {
        /// <summary>
        /// Default interval between keys values sending (175 ms).
        /// </summary>
        public static TimeSpan DefaultKeySendingInterval { get; } = TimeSpan.FromMilliseconds(175);

        /// <summary>
        /// Default page searching timeout (1 min).
        /// </summary>
        public static TimeSpan DefaultPageSearchingTimeout { get; } = TimeSpan.FromSeconds(60);

        /// <summary>
        /// Default element searching timeout (30 sec).
        /// </summary>
        public static TimeSpan DefaultElementSearchingTimeout { get; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Default waiting timeout (1 min).
        /// </summary>
        public static TimeSpan DefaultConditionWaitingTimeout { get; } = TimeSpan.FromSeconds(60);

        /// <summary>
        /// Default interval between condition checking while waiting (1 sec).
        /// </summary>
        public static TimeSpan DefaultConditionCheckingInterval { get; } = TimeSpan.FromMilliseconds(1000);

        /// <summary>
        /// Default time deviation for random waiting (25 %).
        /// </summary>
        public static double DefaultRandomWaitDeviationPercent { get; } = 0.25;



        /// <summary>
        /// Wrapped driver.
        /// </summary>
        public IWebDriver WrappedDriver { get; }

        /// <summary>
        /// Provides access to Robot's elements.
        /// </summary>
        public IRoboElements Elements => new RoboElements(this);

        /// <summary>
        /// Provides access to Robot's browser.
        /// </summary>
        public IRoboBrowser Browser => new RoboBrowser(this);

        /// <summary>
        /// Provides access to Robot's pages.
        /// </summary>
        public IRoboPages Pages => new RoboPages(this);



        /// <summary>
        /// Interval between keys values sending (default 175 ms).
        /// </summary>
        public TimeSpan KeySendingInterval { get; set; }

        /// <summary>
        /// Page searching timeout (default 1 min).
        /// </summary>
        public TimeSpan PageSearchingTimeout { get; set; }

        /// <summary>
        /// Element searching timeout (default 30 sec).
        /// </summary>
        public TimeSpan ElementSearchingTimeout { get; set; }

        /// <summary>
        /// Waiting timeout (default 1 min).
        /// </summary>
        public TimeSpan ConditionWaitingTimeout { get; set; }

        /// <summary>
        /// Interval between condition checking while waiting (default 1 sec).
        /// </summary>
        public TimeSpan ConditionCheckingInterval { get; set; }

        /// <summary>
        /// Time deviation for random waiting (default 25 %).
        /// </summary>
        public double RandomWaitDeviationPercent { get; set; }


        /// <summary>
        /// Cast WebDriver to IJavaScriptExecutor.
        /// </summary>
        public IJavaScriptExecutor JavaScript => (IJavaScriptExecutor)this.WrappedDriver;



        /// <summary>
        /// Crate a new instance with default parameters.
        /// </summary>
        public WebRobot(IWebDriver driver) {
            this.WrappedDriver = driver ?? throw new ArgumentNullException(nameof(driver));
            this.KeySendingInterval = DefaultKeySendingInterval;
            this.PageSearchingTimeout = DefaultPageSearchingTimeout;
            this.ElementSearchingTimeout = DefaultElementSearchingTimeout;
            this.ConditionWaitingTimeout = DefaultConditionWaitingTimeout;
            this.ConditionCheckingInterval = DefaultConditionCheckingInterval;
            this.RandomWaitDeviationPercent = DefaultRandomWaitDeviationPercent;
        }

        /// <summary>
        /// Create a new instance using parameters from the donor robot.
        /// </summary>
        public WebRobot(IRobot robot) {
            if (robot == null) {
                throw new ArgumentNullException(nameof(robot));
            }
            if (robot.WrappedDriver == null) {
                throw new ArgumentException(
                    $"{nameof(IWebDriver)} required",
                    $"{nameof(robot)}.{nameof(this.WrappedDriver)}"
                    );
            }
            this.WrappedDriver = robot.WrappedDriver;
            this.KeySendingInterval = robot.KeySendingInterval;
            this.PageSearchingTimeout = robot.PageSearchingTimeout;
            this.ElementSearchingTimeout = robot.ElementSearchingTimeout;
            this.ConditionWaitingTimeout = robot.ConditionWaitingTimeout;
            this.ConditionCheckingInterval = robot.ConditionCheckingInterval;
            this.RandomWaitDeviationPercent = robot.RandomWaitDeviationPercent;
        }



        public IRoboElement Using(IWebElement element) {
            throw new NotImplementedException();
        }

        public IRoboElement Using(IWebPointer pointer) {
            throw new NotImplementedException();
        }

        public IRoboElement UsingElement(string xpath) {
            throw new NotImplementedException();
        }

        public IRoboElement UsingElement(By locator) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Execute script using WebDriver.
        /// </summary>
        /// <returns>Script result.</returns>
        public object ExecuteJavaScript(string script) {
            return this.JavaScript.ExecuteScript(script);
        }

        /// <summary>
        /// Creates a task that completes after a delay ± % of random deviation.
        /// </summary>
        public Task WaitRandomAsync(Delays delay) {
            return WaitRandomAsync(delay, CancellationToken.None);
        }

        /// <summary>
        /// Creates a task that completes after a delay ± % of random deviation.
        /// </summary>
        public Task WaitRandomAsync(TimeSpan delay) {
            return WaitRandomAsync(delay, CancellationToken.None);
        }

        /// <summary>
        /// Creates a task that completes after a delay ± % of random deviation.
        /// </summary>
        public Task WaitRandomAsync(int milliseconds) {
            return WaitRandomAsync(milliseconds, CancellationToken.None);
        }

        /// <summary>
        /// Creates a task that completes after a delay between range limits.
        /// </summary>
        public Task WaitRandomAsync(Delays from, Delays to) {
            return WaitRandomAsync(from, to, CancellationToken.None);
        }

        /// <summary>
        /// Creates a task that completes after a delay between range limits.
        /// </summary>
        public Task WaitRandomAsync(TimeSpan from, TimeSpan to) {
            return WaitRandomAsync(from, to, CancellationToken.None);
        }

        /// <summary>
        /// Creates a task that completes after a delay between range limits.
        /// </summary>
        public Task WaitRandomAsync(int fromMs, int toMs) {
            return WaitRandomAsync(fromMs, toMs, CancellationToken.None);
        }

        /// <summary>
        /// Creates a task that completes after a delay ± % of random deviation.
        /// </summary>
        public Task WaitRandomAsync(Delays delay, CancellationToken token) {
            return WaitRandomAsync((int)delay, token);
        }

        /// <summary>
        /// Creates a task that completes after a delay ± % of random deviation.
        /// </summary>
        public Task WaitRandomAsync(TimeSpan delay, CancellationToken token) {
            var deviation = RandomGenerator.Default.NextDoubleSign() * this.RandomWaitDeviationPercent;
            var milliseconds = delay.TotalMilliseconds * (1 - deviation);
            return Task.Delay((int)Math.Abs(milliseconds), token);
        }

        /// <summary>
        /// Creates a task that completes after a delay ± % of random deviation.
        /// </summary>
        public Task WaitRandomAsync(int milliseconds, CancellationToken token) {
            return WaitRandomAsync(TimeSpan.FromMilliseconds(milliseconds), token);
        }

        /// <summary>
        /// Creates a task that completes after a delay between range limits.
        /// </summary>
        public Task WaitRandomAsync(Delays from, Delays to, CancellationToken token) {
            return WaitRandomAsync((int)from, (int)to, token);
        }

        /// <summary>
        /// Creates a task that completes after a delay between range limits.
        /// </summary>
        public Task WaitRandomAsync(TimeSpan from, TimeSpan to, CancellationToken token) {
            var min = from.TotalMilliseconds;
            var additive = Math.Abs(to.TotalMilliseconds - min) * RandomGenerator.Default.NextDouble();
            return Task.Delay((int)(min + additive), token);
        }

        /// <summary>
        /// Creates a task that completes after a delay between range limits.
        /// </summary>
        public Task WaitRandomAsync(int fromMs, int toMs, CancellationToken token) {
            return WaitRandomAsync(
                TimeSpan.FromMilliseconds(fromMs),
                TimeSpan.FromMilliseconds(toMs),
                token
                );
        }

        public IRoboWait Wait() {
            throw new NotImplementedException();
        }
    }
}
