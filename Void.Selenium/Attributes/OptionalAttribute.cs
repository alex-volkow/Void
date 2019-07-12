using System;
using System.Collections.Generic;
using System.Text;

namespace Void.Selenium
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class OptionalAttribute : Attribute { }
}
