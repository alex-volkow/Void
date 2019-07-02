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
        /// <summary>
        /// Get URI path to the assembly.
        /// </summary>
        /// <returns>URI path.</returns>
        /// <exception cref="ArgumentNullException">The Assembly is null.</exception>
        public static string GetPath(this Assembly assembly) {
            if (assembly == null) {
                throw new ArgumentNullException(nameof(assembly));
            }
            var uri = new UriBuilder(assembly.CodeBase);
            return Uri.UnescapeDataString(uri.Path);
        }

        /// <summary>
        /// Get the assembly's root namespace.
        /// </summary>
        /// <returns>Root namespace if the assembly is not null else null.</returns>
        public static string GetNamespace(this Assembly assembly) {
            return assembly?.ManifestModule?.Name
                ?.Remove(".dll")
                ?.Remove(".exe");
        }

        /// <summary>
        /// Read embedded resource from the assembly as byte array.
        /// </summary>
        /// <param name="assembly">Source assembly.</param>
        /// <param name="name">Relative or absolute resource name.</param>
        /// <returns>Content as byte array.</returns>
        /// <exception cref="NotFoundException">Resource is not found.</exception>
        /// <exception cref="ArgumentNullException">Assembly is null</exception>
        public static byte[] ReadBytesResource(this Assembly assembly, string name) {
            if (assembly == null) {
                throw new ArgumentNullException(nameof(assembly));
            }
            var path = assembly.GetNamespace();
            if (!name.StartsWith(path)) {
                name = $"{path}.{name}";
            }
            using (var stream = assembly.GetManifestResourceStream(name)) {
                return stream?.ToArray() ?? throw new NotFoundException(
                    $"Resource is not found: {name}"
                    );
            }
        }

        /// <summary>
        /// Read embedded resource from the assembly as UTF-8 string.
        /// </summary>
        /// <param name="assembly">Source assembly.</param>
        /// <param name="name">Relative or absolute resource name.</param>
        /// <returns>Content as string in UTF-8 encoding./returns>
        /// <exception cref="NotFoundException">Resource is not found.</exception>
        /// <exception cref="ArgumentNullException">Encoding is null.</exception>
        /// <exception cref="ArgumentNullException">Assembly is null.</exception>
        public static string ReadStringResource(this Assembly assembly, string name) {
            return assembly.ReadStringResource(name, Encoding.UTF8);
        }

        /// <summary>
        /// Read embedded resource from the assembly as string.
        /// </summary>
        /// <param name="assembly">Source assembly.</param>
        /// <param name="name">Relative or absolute resource name.</param>
        /// <param name="encoding">String encoding.</param>
        /// <returns>Content as string in custom encoding.</returns>
        /// <exception cref="NotFoundException">Resource is not found.</exception>
        /// <exception cref="ArgumentNullException">Encoding is null.</exception>
        /// <exception cref="ArgumentNullException">Assembly is null.</exception>
        public static string ReadStringResource(this Assembly assembly, string name, Encoding encoding) {
            if (encoding == null) {
                throw new ArgumentNullException(nameof(encoding));
            }
            return encoding.GetString(assembly.ReadBytesResource(name));
        }
    }
}
