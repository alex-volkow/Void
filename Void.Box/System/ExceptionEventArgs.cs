using System;
using System.Collections.Generic;
using System.Text;

namespace Void
{
    public class ExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; }

        public ExceptionEventArgs(Exception exception) {
            this.Exception = exception ?? throw new ArgumentNullException(
                nameof(exception)
                );
        }
    }
}
