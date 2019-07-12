using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Selenium
{
    public interface IWebPageElement : IWebPointer
    {
        bool IsOptional { get; }
        bool IsVisible { get; }
    }
}
