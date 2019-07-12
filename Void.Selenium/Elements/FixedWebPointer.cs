using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Void.Selenium
{
    /// <summary>
    /// Represents pointer to dynamic IWebElement.
    /// </summary>
    public class FixedWebPointer : IWebPointer
    {
        /// <summary>
        /// Searching element mechanism.
        /// </summary>
        public By Locator { get; }

        /// <summary>
        /// Container for element searching.
        /// </summary>
        public ISearchContext Context { get; }

        /// <summary>
        /// Found element.
        /// </summary>
        public IWebElement WrappedElement { get; private set; }

        /// <summary>
        /// Check is element found and not stale.
        /// </summary>
        public bool IsMatched => this.WrappedElement != null && !this.IsStaled;

        /// <summary>
        /// Check status of element pointer. If element is stale, need to match it. 
        /// </summary>
        public bool IsStaled {
            get {
                try {
                    return this.WrappedElement != null
                        ? this.WrappedElement.Displayed && false
                        : true;
                }
                catch (StaleElementReferenceException) {
                    return true;
                }
            }
        }



        public FixedWebPointer(ISearchContext context, By locator) {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.Locator = locator ?? throw new ArgumentNullException(nameof(locator));
        }


        /// <summary>
        /// Try to find element in search context with the locator.
        /// </summary>
        /// <returns>Found element or null</returns>
        public IWebElement Match() {
            this.WrappedElement = this.Context
                .FindElements(this.Locator)
                .FirstOrDefault();
            return this.WrappedElement;
        }

        /// <summary>
        /// Provides access to alive element or throw exception.
        /// If element is found and not stale, returns the element.
        /// </summary>
        /// <returns>Match element.</returns>
        /// <exception cref="NotFoundException">Failed to find a matching element.</exception>
        public IWebElement Required() {
            if (this.IsMatched) {
                return this.WrappedElement;
            }
            return Match() ?? throw new NotFoundException(
                $"Element is not found: {this.Locator}"
                );
        }
    }
}
