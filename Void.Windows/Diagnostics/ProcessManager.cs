using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Diagnostics
{
    class ProcessManager : IProcessManager
    {
        public Process Process { get; }

        public ProcessManager(Process process) {
            this.Process = process ??
                throw new ArgumentNullException(
                    nameof(process)
                    );
        }
    }
}
