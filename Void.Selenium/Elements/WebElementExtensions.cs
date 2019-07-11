using Fel.Reflection;
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
        //public static void RefreshRequired(this IDynamicWebElement element) {
        //    element.RefreshRequired($"Element is not found: {element.Locator}");
        //}

        //public static void RefreshRequired(this IDynamicWebElement element, string message) {
        //    element?.Refresh();
        //    if (!(element?.Exists ?? false)) {
        //        throw new OpenQA.Selenium.NotFoundException(
        //            message
        //            );
        //    }
        //}

        //public static void RefreshRequired(this IDynamicWebElement element, TimeSpan timeout) {
        //    element.RefreshRequired($"Element is not found: {element.Locator}", timeout);
        //}

        //public static void RefreshRequired(this IDynamicWebElement element, string message, TimeSpan timeout) {
        //    if (element == null) {
        //        element.RefreshRequired(message);
        //    }
        //    var wait = new WebActionTimeout<OpenQA.Selenium.NotFoundException>(
        //        element.GetWebDriver(), timeout
        //        ) {
        //        Message = message ?? $"Element is not found after {new TimeInterval(timeout)}: {element.Locator}"
        //    };
        //    wait.Until(e => {
        //        element.Refresh();
        //        return element.Exists;
        //    });
        //}

        public static bool IsRednered(this IWebElement element) {
            var result = element.ExecuteJavaScript(IS_RENDERED_SCRIPT);
            return Convert.ToBoolean(result);
        }

        public static bool IsDynamic(this IWebElement element) {
            return element is IDynamicWebElement;
        }

        public static bool IsVisible(this IWebElement element) {
            return element != null && element.Displayed && !element.Size.IsEmpty;
        }

        public static void MouseOver(this IWebElement element) {
            new Actions(element.GetWebDriver())
                .MoveToElement(element)
                .Build()
                .Perform();
        }

        public static void MoveTo(this IWebElement element) {
            var script = "var viewPortHeight = Math.max(document.documentElement.clientHeight, window.innerHeight || 0);"
                + "var elementTop = arguments[0].getBoundingClientRect().top;"
                + "window.scrollBy(0, elementTop-(viewPortHeight/2));";
            element.ExecuteJavaScript(script);
        }

        public static void SetAttribute(this IWebElement element, string attribute, string value) {
            var driver = element.GetWebDriver();
            var javascript = driver.ToJavaScriptExecutor();
            //element.TryRefresh();
            javascript.ExecuteScript(
                "arguments[0].setAttribute(arguments[1], arguments[2])", 
                element, attribute, value
                );
        }

        public static IWebDriver GetRequiredWebDriver(this IWebElement element) {
            var driver = element.GetWebDriver();
            if (driver == null) {
                if (element is IDynamicWebElement dynamicElement) {
                    throw new NotFoundException($"Element is not found: {dynamicElement.Locator}");
                }
                var type = element.GetType().GetNameWithAssemblies();
                throw new InvalidOperationException(
                    $"Failed to get {nameof(IWebDriver)} from the element {type}"
                );
            }
            return driver;
        }

        public static IWebDriver GetWebDriver(this IWebElement element) {
            //if (element is IDynamicWebElement dymanicElement) {
            //    if (dymanicElement.Context is IWebDriver driver) {
            //        return driver;
            //    }
            //    var source = (IWebElement)dymanicElement.Context;
            //    return source.GetWebDriver();
            //}
            if (element is IWrapsDriver driverWrapper) {
                return driverWrapper.WrappedDriver;
            }
            else if (element is IWrapsElement elementWrapper) {
                return elementWrapper.WrappedElement?.GetWebDriver();
                //return elementWrapper.WrappedElement?.GetWebDriver();
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

        public static void SendKeys(this IWebElement element, string value, Engine engine) {
            if (engine == Engine.JavaScript) {
                var image = Strings.ToJavaScript(value);
                element.ExecuteJavaScript($"arguments[0].value=arguments[0].value+'{image}'");
                //var prefix = append ? "arguments[0].value+" : string.Empty;
                //element.ExecuteJavaScript($"arguments[0].value={prefix}'{image}'");
            }
            else if (engine == Engine.Standard) {
                element.SendKeys(value);
            }
            else {
                throw new NotSupportedException(
                    $"Unsupported engine: {engine}"
                    );
            }
        }

        public static void Clear(this IWebElement element, Engine engine) {
            if (engine == Engine.JavaScript) {
                element.ExecuteJavaScript($"arguments[0].value=''");
            }
            else if (engine == Engine.Standard) {
                element.Clear();
            }
            else {
                throw new NotSupportedException(
                    $"Unsupported engine: {engine}"
                    );
            }
        }

        public static void Click(this IWebElement element, Engine engine) {
            if (engine == Engine.JavaScript) {
                element.ExecuteJavaScript("arguments[0].click()");
            }
            else if (engine == Engine.Standard) {
                element.Click();
            }
            else {
                throw new NotSupportedException(
                    $"Unsupported engine: {engine}"
                    );
            }
        }

        public static void Submit(this IWebElement element, Engine engine) {
            if (engine == Engine.JavaScript) {
                element.ExecuteJavaScript("arguments[0].submit()");
            }
            else if (engine == Engine.Standard) {
                element.Submit();
            }
            else {
                throw new NotSupportedException(
                    $"Unsupported engine: {engine}"
                    );
            }
        }

        public static object ExecuteJavaScript(this IWebElement element, string script) {
            return element
                .GetRequiredWebDriver()
                .ToJavaScriptExecutor()
                .ExecuteScript(script, element);
        }

        //internal static bool TryRefresh(this IWebElement element) {
        //    if (element is IDynamicWebElement wrapper) {
        //        wrapper.Refresh();
        //        return wrapper.Exists;
        //    }
        //    return false;
        //}
    }
}
