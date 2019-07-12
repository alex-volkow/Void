using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Selenium
{
    public interface IWebPageElement : IWrapsElement
    {
        bool IsOptional { get; }
        bool IsVisible { get; }
    }
}
