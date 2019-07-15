using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Void.Selenium
{
    public interface IRoboElement : IWebPointer
    {
        /// <summary>
        /// Move to the element, clear it and append text with robot's key sending interval. 
        /// Prevents pointer staling.
        /// </summary>
        Task<IRoboElement> SetText(string text);

        /// <summary>
        /// Move to the element, clear it and append text with robot's key sending interval. 
        /// Prevents pointer staling.
        /// </summary>
        /// <exception cref="TaskCanceledException">Task has been cancelled.</exception>
        Task<IRoboElement> SetText(string text, TimeSpan duration, CancellationToken token);

        /// <summary>
        /// Move to the element and append text with robot's key sending interval. 
        /// Prevents pointer staling.
        /// </summary>
        Task<IRoboElement> AppendText(string text);

        /// <summary>
        /// Move to the element and append text with robot's key sending interval. 
        /// Prevents pointer staling.
        /// </summary>
        /// <exception cref="TaskCanceledException">Task has been cancelled.</exception>
        Task<IRoboElement> AppendText(string text, TimeSpan duration, CancellationToken token);

        /// <summary>
        /// Move to the element and sumbit it.
        /// </summary>
        IRoboElement Submit();

        /// <summary>
        /// Move to the element and click on it.
        /// </summary>
        IRoboElement Click();

        /// <summary>
        /// Move to the element and clear it.
        /// </summary>
        IRoboElement Clear();

        /// <summary>
        /// Move to the element and emulate 'mouse over' event.
        /// </summary>
        IRoboElement MouseOver();

        /// <summary>
        /// Default the robot trying to use standard element methods, but if element is not visible
        /// or error raised, robot will use JavaScript analogs. 
        /// This option disables standard methods using.
        /// </summary>
        /// <returns></returns>
        IRoboElement WithJavaScript();
    }
}
