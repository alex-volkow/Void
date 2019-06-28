using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Void.Diagnostics
{
    public static class Cmd
    {
        [DllImport("shell32.dll", SetLastError = true)]
        static extern IntPtr CommandLineToArgvW(
            [MarshalAs(UnmanagedType.LPWStr)] string lpCmdLine,
            out int pNumArgs
            );

        public static string[] ToArray(string arguments) {
            var argv = CommandLineToArgvW(arguments, out int argc);
            if (argv == IntPtr.Zero) {
                throw new Win32Exception();
            }
            try {
                var args = new string[argc];
                for (var i = 0; i < args.Length; i++) {
                    var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(p);
                }
                return args;
            }
            finally {
                Marshal.FreeHGlobal(argv);
            }
        }
    }
}
