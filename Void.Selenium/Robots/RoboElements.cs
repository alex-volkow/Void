using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Void.Selenium
{
    class RoboElements : RoboComponent, IRoboElements
    {
        public ISearchContext Context { get; }



        public RoboElements(IRobot robot)
            : this(robot, robot.WrappedDriver) {
        }

        public RoboElements(IRobot robot, ISearchContext context) 
            : base(robot) {
            this.Context = context ?? robot.WrappedDriver;
        }


        public IRoboElements In(ISearchContext context) {
            return new RoboElements(this.Robot, context);
        }

        public IRoboElement Find(By locator) {
            if (locator == null) {
                throw new ArgumentNullException(
                    nameof(locator)
                    );
            }
            var pointer = new FixedWebPointer(this.Context, locator);
            if (pointer.Match() == null) {
                throw new NotFoundException(
                    $"Element is not found: {locator}"
                    );
            }
            return new RoboWebPointer(this.Robot, pointer);
        }

        public IRoboElement FindByXpath(string xpath) {
            return Find(By.XPath(xpath));
        }

        public IRoboElement TryFind(By locator) {
            if (locator == null) {
                throw new ArgumentNullException(
                    nameof(locator)
                    );
            }
            var pointer = new FixedWebPointer(this.Context, locator);
            pointer.Match();
            return new RoboWebPointer(this.Robot, pointer);
        }

        public IRoboElement TryFindByXpath(string xpath) {
            return TryFind(By.XPath(xpath));
        }

        public IEnumerable<IRoboElement> FindAll(By locator) {
            return this.Context.FindElements(locator)
                .Select(e => new RoboFixedElement(this.Robot, e))
                .ToArray();
        }

        public IEnumerable<IRoboElement> FindAllByXpath(string xpath) {
            return FindAll(By.XPath(xpath));
        }

        public Task<IRoboElement> FindAsync(By locator) {
            return FindAsync(locator, this.Robot.ElementSearchingTimeout);
        }

        public Task<IRoboElement> FindAsync(By locator, TimeSpan timeout) {
            return FindAsync(locator, timeout, CancellationToken.None);
        }

        public async Task<IRoboElement> FindAsync(By locator, TimeSpan timeout, CancellationToken token) {
            var element = await TryFindAsync(locator, timeout, token);
            if (!element.Exists) {
                throw new NotFoundException(
                    $"Element is not found: {locator}"
                    );
            }
            return element;
        }

        public Task<IRoboElement> FindByXpathAsync(string xpath) {
            return FindAsync(By.XPath(xpath), this.Robot.ElementSearchingTimeout);
        }

        public Task<IRoboElement> FindByXpathAsync(string xpath, TimeSpan timeout) {
            return FindAsync(By.XPath(xpath), timeout, CancellationToken.None);
        }

        public Task<IRoboElement> TryFindAsync(By locator) {
            return TryFindAsync(locator, this.Robot.ElementSearchingTimeout);
        }

        public Task<IRoboElement> TryFindAsync(By locator, TimeSpan timeout) {
            return TryFindAsync(locator, timeout, CancellationToken.None);
        }

        public async Task<IRoboElement> TryFindAsync(By locator, TimeSpan timeout, CancellationToken token) {
            if (locator == null) {
                throw new ArgumentNullException(nameof(locator));
            }
            var pointer = new FixedWebPointer(this.Context, locator);
            var element = await this.Robot.Wait()
                .UsingExceptionHandler(e => true)
                .IgnoreConditionExceptions()
                .NotThrowTimeoutException()
                .WithTimeout(timeout)
                .UntilAsync(() => {
                    if (pointer.Match() != null) {
                        return (IRoboElement)new RoboWebPointer(this.Robot, pointer);
                    }
                    return null;
                });
            return element ?? new RoboWebPointer(this.Robot, pointer);
        }

        public Task<IRoboElement> TryFindByXpathAsync(string xpath) {
            return TryFindAsync(By.XPath(xpath), this.Robot.ElementSearchingTimeout);
        }

        public Task<IRoboElement> TryFindByXpathAsync(string xpath, TimeSpan timeout) {
            return TryFindAsync(By.XPath(xpath), timeout);
        }
    }
}
