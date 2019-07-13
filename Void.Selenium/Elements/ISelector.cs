using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Selenium
{
    public interface ISelector : IWrapsElement
    {
        bool IsMultiple { get; }

        void DeselectAll();
        void DeselectByIndex(int index);
        void DeselectByText(string text);
        void DeselectByValue(string value);
        void SelectByIndex(int index);
        void SelectByText(string text);
        void SelectByValue(string value);
        IWebElement GetSelectedElement();
        IReadOnlyList<IOptions> GetOptions();
        IReadOnlyList<IWebElement> GetElements();
        IReadOnlyList<IWebElement> GetSelectedElements();
    }
}
