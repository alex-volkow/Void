using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Void.Reflection
{
    public static class FieldsExtensions
    {
        /// <summary>
        /// Check the field belongs auto-property.
        /// </summary>
        public static bool IsAutoField(this FieldInfo field) {
            if (field == null) {
                throw new ArgumentNullException(
                    nameof(field)
                    );
            }
            return field.Name.StartsWith("<")
                && field.Name.EndsWith(">k__BackingField");
        }

        /// <summary>
        /// Get field auto-property name.
        /// </summary>
        /// <returns>Name if field belongs auto-property else null.</returns>
        public static string GetAutoPropertyName(this FieldInfo field) {
            if (field == null) {
                throw new ArgumentNullException(
                    nameof(field)
                    );
            }
            var name = field.Name.Remove(">k__BackingField");
            if (name.Length < field.Name.Length) {
                return name.Remove("<");
            }
            return null;
        }
    }
}
