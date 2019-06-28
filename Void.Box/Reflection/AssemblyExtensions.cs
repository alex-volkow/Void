using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Void.IO;

namespace Void.Reflection
{
    public static class AssemblyExtensions
    {
        public static string GetPath(this Assembly assembly) {
            var uri = new UriBuilder(assembly.CodeBase);
            return Uri.UnescapeDataString(uri.Path);
        }

        public static string GetNamespace(this Assembly assembly) {
            return assembly?.ManifestModule?.Name
                ?.Remove(".dll")
                ?.Remove(".exe");
        }

        public static byte[] ReadBytesResource(this Assembly assembly, string name) {
            var path = assembly.GetNamespace();
            if (!name.StartsWith(path)) {
                name = $"{path}.{name}";
            }
            using (var stream = assembly.GetManifestResourceStream(name)) {
                return stream.ToArray();
            }
        }

        public static string ReadStringResource(this Assembly assembly, string name) {
            return assembly.ReadStringResource(name, Encoding.UTF8);
        }

        public static string ReadStringResource(this Assembly assembly, string name, Encoding encoding) {
            return encoding.GetString(assembly.ReadBytesResource(name));
        }
    }
}
