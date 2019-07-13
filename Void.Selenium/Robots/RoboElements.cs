using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Void.Selenium
{
    class RoboElements : RoboComponent, IRoboElements
    {
        public RoboElements(IRobot robot) 
            : base(robot) {
        }

        public Task<IWebPointer> FindFirstAsync(By locator) {
            throw new NotImplementedException();
        }

        public Task<IWebPointer> FindFirstAsync(By locator, TimeSpan timeout) {
            throw new NotImplementedException();
        }

        public Task<IWebPointer> FindFirstByXpathAsync(string xpath) {
            throw new NotImplementedException();
        }

        public Task<IWebPointer> FindFirstByXpathAsync(string xpath, TimeSpan timeout) {
            throw new NotImplementedException();
        }

        public IRoboElements In(ISearchContext context) {
            throw new NotImplementedException();
        }

        public Task<IWebPointer> TryFindFirstAsync(By locator) {
            throw new NotImplementedException();
        }

        public Task<IWebPointer> TryFindFirstAsync(By locator, TimeSpan timeout) {
            throw new NotImplementedException();
        }

        public Task<IWebPointer> TryFindFirstByXpathAsync(string xpath) {
            throw new NotImplementedException();
        }

        public Task<IWebPointer> TryFindFirstByXpathAsync(string xpath, TimeSpan timeout) {
            throw new NotImplementedException();
        }
    }
}
