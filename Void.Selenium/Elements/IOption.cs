using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Selenium
{
    public interface IOption : IWrapsElement
    {
        bool Selected { get; }
        string Value { get; }
        string Text { get; }
        int Index { get; }

        void Select();
        void Deselect();
    }
}
