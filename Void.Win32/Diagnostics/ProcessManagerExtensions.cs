using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Void.Diagnostics
{
    public static class ProcessManagerExtensions
    {
        private const int SW_HIDE = 0;
        private const int SW_NORMAL = 1;



        [DllImport("User32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);



        public static void Hide(this IProcessManager handler) {
            ShowWindow(handler.Process.MainWindowHandle, SW_HIDE);
        }

        public static void Show(this IProcessManager handler) {
            ShowWindow(handler.Process.MainWindowHandle, SW_NORMAL);
        }
    }
}
