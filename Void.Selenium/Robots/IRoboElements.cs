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
        //Task<IDynamicWebElement> FindElement(ISearchContext context, By locator);
        //Task<IDynamicWebElement> TryFindElement(ISearchContext context, By locator);
        //Task<IDynamicWebElement> FindElement(ISearchContext context, By locator, TimeSpan timeout);
        //Task<IDynamicWebElement> TryFindElement(ISearchContext context, By locator, TimeSpan timeout);

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
