using System;
using System.Collections.Generic;
using System.Text;

namespace Void
{
    public interface IConsoleProgressBar : IDisposable, IProgress<double>
    {
        int Size { get; }
        double Progress { get; set; }
        string DigitFormat { get; set; }
        bool IsSpinnerEnabled { get; set; }
        bool IsSpinnerClockwise { get; set; }
        TimeSpan SpinnerInterval { get; set; }
    }
}
