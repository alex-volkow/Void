using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Void
{
    public interface IConsoleSpinner : IDisposable
    {
        bool Clockwise { get; }
        TimeSpan Interval { get; }


        void Start();
    }
}
