using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Void.Reflection
{
    public static class FieldsExtensions
    {
        public static bool IsAutoField(this FieldInfo field) {
            if (field == null) {
                throw new ArgumentNullException(
                    nameof(field)
                    );
            }
            return field.Name.StartsWith("<")
                && field.Name.EndsWith(">k__BackingField");
        }

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
