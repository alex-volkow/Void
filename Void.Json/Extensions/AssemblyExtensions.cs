using Void.Reflection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Void.Json
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<JsonConverter> GetJsonConverters(this Assembly assembly) {
            return (assembly ?? throw new ArgumentNullException(nameof(assembly)))
                .GetTypes()
                .Where(e => e.Is<JsonConverter>())
                .Where(e => !e.IsAbstract)
                .Where(e => e.HasDefaultConstructor())
                .Select(e => Activator.CreateInstance(e))
                .Cast<JsonConverter>();
        }
    }
}
