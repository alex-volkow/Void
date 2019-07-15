using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Void.Diagnostics
{
    public static class ProcessExtensions
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);



        public static void FocusWindow(this Process process) {
            SetForegroundWindow(process.MainWindowHandle);
        }

        public static IProcessManager Manage(this Process process) {
            return new ProcessManager(process);
        }

        public static IEnumerable<Process> GetDescendants(this Process process) {
            var descendants = new List<Process>();
            var children = process.TryGetChildren();
            descendants.AddRange(children);
            foreach (var child in children) {
                descendants.AddRange(child.GetDescendants());
            }
            return descendants;
        }

        public static IEnumerable<Process> GetChildren(this Process process) {
            var query = $"SELECT * FROM Win32_Process WHERE ParentProcessId={process.Id}";
            using (var searcher = new ManagementObjectSearcher(query)) {
                return searcher.Get()
                    .Cast<ManagementObject>()
                    .Select(item => TryGetProcess((uint)item["ProcessId"]))
                    .Where(item => item != null);
            }
        }

        public static Process GetParent(this Process process) {
            var query = $"SELECT * FROM Win32_Process WHERE ProcessId={process.Id}";
            using (var searcher = new ManagementObjectSearcher(query)) {
                return searcher.Get()
                  .OfType<ManagementObject>()
                  .Select(item => TryGetProcess((uint)item["ParentProcessId"]))
                  .Where(item => item != null)
                  .FirstOrDefault();
            }
        }

        private static IEnumerable<Process> TryGetChildren(this Process process) {
            try {
                return process.GetChildren();
            }
            catch {
                return new Process[] { };
            }
        }

        private static Process TryGetProcess(uint id) {
            return Process.GetProcesses().FirstOrDefault(e => e.Id == id);
        }
    }
}
