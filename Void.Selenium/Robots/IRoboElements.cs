using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Void.Selenium
{
    public interface IRoboElements
    {
        /// <summary>
        /// Using custom search context (default WebDriver).
        /// </summary>
        IRoboElements In(ISearchContext context);

        /// <summary>
        /// Create robo element with locator pointer.
        /// </summary>
        /// <returns>Not null value.</returns>
        /// <exception cref="ArgumentNullException">Locator is null.</exception>
        /// <exception cref="NotFoundException">Element is not found.</exception>
        IRoboElement Find(By locator);

        /// <summary>
        /// Create robo element with xpath pointer.
        /// </summary>
        /// <returns>Not null value.</returns>
        /// <exception cref="NotFoundException">Element is not found.</exception>
        IRoboElement FindByXpath(string xpath);

        /// <summary>
        /// Try to find element by locator pointer.
        /// </summary>
        /// <returns>Not null value.</returns>
        /// <exception cref="ArgumentNullException">Locator is null.</exception>
        IRoboElement TryFind(By locator);

        /// <summary>
        /// Try to find element by xpath pointer.
        /// </summary>
        /// <returns>Not null value.</returns>
        IRoboElement TryFindByXpath(string xpath);

        /// <summary>
        /// Find all available elements by locator.
        /// </summary>
        /// <returns>Not null value.</returns>
        IEnumerable<IRoboElement> FindAll(By locator);

        /// <summary>
        /// Find all available elements by xpath.
        /// </summary>
        /// <returns>Not null value.</returns>
        IEnumerable<IRoboElement> FindAllByXpath(string xpath);

        /// <summary>
        /// Find element asynchronously by locator pointer
        /// with robot's element search timeout.
        /// </summary>
        /// <returns>Not null value.</returns>
        /// <exception cref="ArgumentNullException">Locator is null.</exception>
        /// <exception cref="NotFoundException">Element is not found.</exception>
        Task<IRoboElement> FindAsync(By locator);

        /// <summary>
        /// Find element asynchronously by locator pointer.
        /// </summary>
        /// <returns>Not null value.</returns>
        /// <exception cref="ArgumentNullException">Locator is null.</exception>
        /// <exception cref="NotFoundException">Element is not found.</exception>
        Task<IRoboElement> FindAsync(By locator, TimeSpan timeout);

        /// <summary>
        /// Find element asynchronously by locator pointer.
        /// </summary>
        /// <returns>Not null value.</returns>
        /// <exception cref="ArgumentNullException">Locator is null.</exception>
        /// <exception cref="NotFoundException">Element is not found.</exception>
        Task<IRoboElement> FindAsync(By locator, TimeSpan timeout, CancellationToken token);

        /// <summary>
        /// Find element asynchronously by xpath pointer
        /// with robot's element search timeout.
        /// </summary>
        /// <returns>Not null value.</returns>
        /// <exception cref="NotFoundException">Element is not found.</exception>
        Task<IRoboElement> FindByXpathAsync(string xpath);

        /// <summary>
        /// Find element asynchronously by xpath pointer.
        /// </summary>
        /// <returns>Not null value.</returns>
        /// <exception cref="NotFoundException">Element is not found.</exception>
        Task<IRoboElement> FindByXpathAsync(string xpath, TimeSpan timeout);


        /// <summary>
        /// Try to find element asynchronously by locator pointer
        /// with robot's element search timeout.
        /// </summary>
        /// <returns>Not null value.</returns>
        /// <exception cref="ArgumentNullException">Locator is null.</exception>
        Task<IRoboElement> TryFindAsync(By locator);

        /// <summary>
        /// Try to find element asynchronously by locator pointer.
        /// </summary>
        /// <returns>Not null value.</returns>
        /// <exception cref="ArgumentNullException">Locator is null.</exception>
        Task<IRoboElement> TryFindAsync(By locator, TimeSpan timeout);

        /// <summary>
        /// Try to find element asynchronously by locator pointer.
        /// </summary>
        /// <returns>Not null value.</returns>
        /// <exception cref="ArgumentNullException">Locator is null.</exception>
        Task<IRoboElement> TryFindAsync(By locator, TimeSpan timeout, CancellationToken token);

        /// <summary>
        /// Try to find element asynchronously by xpath pointer
        /// with robot's element search timeout.
        /// </summary>
        /// <returns>Not null value.</returns>
        Task<IRoboElement> TryFindByXpathAsync(string xpath);

        /// <summary>
        /// Try to find element asynchronously by xpath pointer.
        /// </summary>
        /// <returns>Not null value.</returns>
        Task<IRoboElement> TryFindByXpathAsync(string xpath, TimeSpan timeout);
    }
}
