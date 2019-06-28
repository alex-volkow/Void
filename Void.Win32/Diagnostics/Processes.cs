using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;

namespace Void.Diagnostics
{
    public static class Processes
    {
        public static IEnumerable<Process> GetAlive(IEnumerable<int> ids) {
            foreach (var process in Process.GetProcesses()) {
                if (ids.Any(e => e == process.Id)) {
                    yield return process;
                }
            }
        }

        public static IEnumerable<Process> GetDescendants(int parentId) {
            var descendants = new List<Process>();
            var children = GetChildren(parentId);
            descendants.AddRange(children);
            foreach (var child in children) {
                descendants.AddRange(
                    GetDescendants(child.Id)
                    );
            }
            return descendants;
        }

        public static IEnumerable<Process> GetChildren(int parentId) {
            try {
                var query = $"SELECT * FROM Win32_Process WHERE ParentProcessId={parentId}";
                using (var searcher = new ManagementObjectSearcher(query)) {
                    return searcher.Get()
                        .Cast<ManagementObject>()
                        .Select(item => GetProcess((uint)item["ProcessId"]))
                        .Where(item => item != null);
                }
            }
            catch {
                return new Process[] { };
            }
        }

        public static Process GetParent(int processId) {
            try {
                var query = $"SELECT * FROM Win32_Process WHERE ProcessId={processId}";
                using (var searcher = new ManagementObjectSearcher(query)) {
                    return searcher.Get()
                      .OfType<ManagementObject>()
                      .Select(item => GetProcess((uint)item["ParentProcessId"]))
                      .Where(item => item != null)
                      .FirstOrDefault();
                }
            }
            catch {
                return null;
            }
        }

        public static Process GetProcess(int id) {
            return GetProcess((uint)id);
        }

        public static Process GetProcess(uint id) {
            return Process.GetProcesses().FirstOrDefault(e => e.Id == id);
        }
    }
}
