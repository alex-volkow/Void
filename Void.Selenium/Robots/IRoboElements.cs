using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Void.Selenium
{
    public interface IRoboElements
    {
        IRoboElements In(ISearchContext context);

        IRoboElement Find(By locator);
        IRoboElement FindByXpath(string xpath);
        IRoboElement FindFirst(By locator);
        IRoboElement FindFirstByXpath(string xpath);
        IEnumerable<IRoboElement> FindAll(By locator);
        IEnumerable<IRoboElement> FindAllByXpath(string xpath);

        Task<IWebPointer> FindFirstAsync(By locator);
        Task<IWebPointer> FindFirstAsync(By locator, TimeSpan timeout);
        Task<IWebPointer> FindFirstByXpathAsync(string xpath);
        Task<IWebPointer> FindFirstByXpathAsync(string xpath, TimeSpan timeout);

        Task<IWebPointer> TryFindFirstAsync(By locator);
        Task<IWebPointer> TryFindFirstAsync(By locator, TimeSpan timeout);
        Task<IWebPointer> TryFindFirstByXpathAsync(string xpath);
        Task<IWebPointer> TryFindFirstByXpathAsync(string xpath, TimeSpan timeout);
    }
}
