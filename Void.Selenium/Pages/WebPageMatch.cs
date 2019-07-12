using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Void.Selenium
{
    class WebPageMatch : IWebPageMatch
    {
        public bool Success => !this.Errors.Any();

        public List<string> Errors { get; } = new List<string>();

        IEnumerable<string> IWebPageMatch.Errors => this.Errors;
    }
}
