using Void.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Selenium
{
    public static class WebElementExtensions
    {
        private static readonly string IS_RENDERED_SCRIPT =
                "var elem = arguments[0],                 " +
                "  box = elem.getBoundingClientRect(),    " +
                "  cx = box.left + box.width / 2,         " +
                "  cy = box.top + box.height / 2,         " +
                "  e = document.elementFromPoint(cx, cy); " +
                "for (; e; e = e.parentElement) {         " +
                "  if (e === elem)                        " +
                "    return true;                         " +
                "}                                        " +
                "return false;                            ";

        /// <summary>
        /// Check the element is rendered in a browser with JavaScript.
        /// </summary>
        /// <exception cref="ArgumentNullException">Element is null.</exception>
        public static bool IsRednered(this IWebElement element) {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }
            return Convert.ToBoolean(element.ExecuteJavaScript(IS_RENDERED_SCRIPT));
        }

        /// <summary>
        /// Check the element is visible in UI.
        /// </summary>
        /// <exception cref="ArgumentNullException">Element is null.</exception>
        public static bool IsVisible(this IWebElement element) {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }
            try {
                return element.Displayed 
                    && !element.Size.IsEmpty;
            }
            catch (StaleElementReferenceException) {
                return false;
            }
        }

        /// <summary>
        /// Imitate mouse cursor over the element.
        /// </summary>
        /// <exception cref="ArgumentNullException">Element is null.</exception>
        public static void MouseOver(this IWebElement element) {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }
            new Actions(element.GetWebDriver())
                .MoveToElement(element)
                .Build()
                .Perform();
        }

        /// <summary>
        /// Scroll a screen to the element with JavaScript.
        /// </summary>
        /// <exception cref="ArgumentNullException">Element is null.</exception>
        public static void MoveTo(this IWebElement element) {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }
            var script = "var viewPortHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);"
                + "var elementTop = arguments[0].getBoundingClientRect().top;"
                + "window.scrollBy(0, elementTop-(viewPortHeight/2));";
            element.ExecuteJavaScript(script);
        }

        /// <summary>
        /// Set the element's attribute.
        /// </summary>
        /// <param name="element">Target element.</param>
        /// <param name="attribute">Attribute name.</param>
        /// <param name="value">Attribute value.</param>
        /// <exception cref="ArgumentNullException">Element or attribute are null</exception>
        public static void SetAttribute(this IWebElement element, string attribute, string value) {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }
            if (attribute == null) {
                throw new ArgumentNullException(nameof(attribute));
            }
            var driver = element.GetWebDriver();
            var javascript = (IJavaScriptExecutor)driver;
            javascript.ExecuteScript(
                "arguments[0].setAttribute(arguments[1], arguments[2])", 
                element, attribute, value ?? string.Empty
                );
        }

        /// <summary>
        /// Unwrap a web driver from the element.
        /// </summary>
        /// <exception cref="ArgumentNullException">Element is null.</exception>
        /// <exception cref="NotFoundException">Web driver is not found.</exception>
        public static IWebDriver GetRequiredWebDriver(this IWebElement element) {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }
            return element.GetWebDriver() ?? throw new NotFoundException(
                $"Element does not wrap the {nameof(IWebDriver)}"
                );
        }

        /// <summary>
        /// Unwrap a web driver from the element.
        /// </summary>
        /// <returns>Found web driver or null if not found.</returns>
        /// <exception cref="ArgumentNullException">Element is null.</exception>
        public static IWebDriver GetWebDriver(this IWebElement element) {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }
            if (element is IWrapsDriver driverWrapper) {
                return driverWrapper.WrappedDriver;
            }
            else if (element is IWrapsElement elementWrapper) {
                return elementWrapper.WrappedElement?.GetWebDriver();
            }
            else {
                var property = element.GetType().GetProperty("WrappedElement");
                if (property != null) {
                    if (property.PropertyType.Is<IWrapsDriver>()) {
                        var wrapper = (IWrapsDriver)property.GetValue(element, null);
                        return wrapper?.WrappedDriver;
                    }
                    if (property.PropertyType.Is<IWrapsElement>()) {
                        var wrapper = (IWrapsElement)property.GetValue(element, null);
                        return wrapper?.WrappedElement?.GetWebDriver();
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Set the element's value with JavaScript.
        /// </summary>
        /// <param name="element">Target element.</param>
        /// <param name="value">Text content.</param>
        /// <exception cref="ArgumentNullException">Element is null</exception>
        public static void SendKeysJavaScript(this IWebElement element, string value) {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }
            if (!string.IsNullOrEmpty(value)) {
                var image = Strings.ToJavaScript(value);
                element.ExecuteJavaScript($"arguments[0].value=arguments[0].value+'{image}'");
            }
        }

        /// <summary>
        /// Clear the element's value with JavaScript.
        /// </summary>
        /// <exception cref="ArgumentNullException">Element is null.</exception>
        public static void ClearJavaScript(this IWebElement element) {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }
            element.ExecuteJavaScript($"arguments[0].value=''");
        }

        /// <summary>
        /// Click on the element with JavaScript.
        /// </summary>
        /// <exception cref="ArgumentNullException">Element is null.</exception>
        public static void ClickJavaScript(this IWebElement element) {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }
            element.ExecuteJavaScript("arguments[0].click()");
        }

        /// <summary>
        /// Perform submit action on the element with JavaScript.
        /// </summary>
        /// <exception cref="ArgumentNullException">Element is null.</exception>
        public static void SubmitJavaScript(this IWebElement element) {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }
            element.ExecuteJavaScript("arguments[0].submit()");
        }

        /// <summary>
        /// Execute the script on the element using JavaScript
        /// where parameter 'arguments[0]' contains the element.
        /// </summary>
        /// <param name="element">Target element.</param>
        /// <param name="script">Script content.</param>
        /// <returns>JavaScript execution result.</returns>
        public static object ExecuteJavaScript(this IWebElement element, string script) {
            if (element == null) {
                throw new ArgumentNullException(nameof(element));
            }
            if (string.IsNullOrWhiteSpace(script)) {
                return null;
            }
            var engine = (IJavaScriptExecutor)element.GetRequiredWebDriver();
            return engine.ExecuteScript(script, element);
        }
    }
}
