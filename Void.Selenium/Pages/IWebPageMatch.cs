using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Selenium
{
    public interface IWebPageMatch
    {
        bool Success { get; }
        IEnumerable<string> Errors { get; }
    }
}
