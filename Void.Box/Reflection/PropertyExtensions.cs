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
        /// <summary>
        /// Try to get field of auto-property.
        /// </summary>
        /// <returns>FieldInfo is field exists else null.</returns>
        public static FieldInfo GetAutoField(this PropertyInfo property) {
            return property.DeclaringType.GetField(
                $"<{property.Name}>k__BackingField",
                BindingFlags.NonPublic |
                BindingFlags.Instance
                );
        }

        /// <summary>
        /// Check the property has auto-field.
        /// </summary>
        public static bool IsAuto(this PropertyInfo property) {
            return property.GetAutoField() != null;
        }

        /// <summary>
        /// Try to set value to the object's property else throw a error.
        /// </summary>
        /// <param name="property">Object's property</param>
        /// <param name="obj">Target object</param>
        /// <param name="value">Value to be setted</param>
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
