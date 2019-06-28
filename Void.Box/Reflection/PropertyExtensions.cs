using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Void.Reflection
{
    public static class PropertyExtensions
    {
        public static FieldInfo GetAutoField(this PropertyInfo property) {
            return property.DeclaringType.GetField(
                $"<{property.Name}>k__BackingField",
                BindingFlags.NonPublic |
                BindingFlags.Instance
                );
        }

        public static bool IsAutoProperty(this PropertyInfo property) {
            return property.GetAutoField() != null;
        }

        public static void SetForce(this PropertyInfo property, object obj, object value) {
            if (property.CanWrite) {
                property.SetValue(obj, value);
                return;
            }
            var autofield = property.GetAutoField();
            if (autofield != null) {
                autofield.SetValue(obj, value);
                return;
            }
            property.SetValue(obj, value);
        }
    }
}
