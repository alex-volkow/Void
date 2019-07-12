using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Void.Reflection
{
    public static class TypeExtensions
    {
        private static readonly Regex TYPE_NAME_WITH_NAMESPACES = new Regex(@",\s*[^,\s\[\]]+", RegexOptions.Compiled);
        private static readonly Regex TYPE_NAME_WITH_ASSEMBLIES = new Regex(@"(,\s*)?((Version)|(Culture)|(PublicKeyToken))=[\w_\.]*", RegexOptions.Compiled);


        public static PropertyInfo GetPropertyInfo<TSource, TProperty>(
            this TSource source,
            Expression<Func<TSource, TProperty>> property) {
            var type = typeof(TSource);
            if (!(property.Body is MemberExpression member)) {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    property.ToString()
                    ));
            }
            var info = member.Member as PropertyInfo;
            if (info == null) {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    property.ToString()
                    ));
            }
            if (type != info.ReflectedType && !type.IsSubclassOf(info.ReflectedType)) {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a property that is not from type {1}.",
                    property.ToString(), type
                    ));
            }
            return info;
        }

        public static string GetAssemblyLocation(this Type type) {
            return type.Assembly.GetPath();
        }

        public static string GetNameWithAssemblies(this Type type) {
            return TYPE_NAME_WITH_ASSEMBLIES.Replace(type.AssemblyQualifiedName, string.Empty);
        }

        public static string GetNameWithNamespaces(this Type type) {
            return TYPE_NAME_WITH_NAMESPACES.Replace(type.AssemblyQualifiedName, string.Empty);
        }

        public static bool IsNullable(this Type type) {
            return type != null
                && type.IsGenericType &&
                type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

        public static bool IsGeneric(this Type type, params Type[] parameters) {
            if (type?.IsGenericType ?? false) {
                if (parameters.Length == 0) {
                    return true;
                }
                var types = type.GetGenericArguments();
                if (types.Length != parameters.Length) {
                    return false;
                }
                for (var i = 0; i < types.Length; i++) {
                    if (types[i] != parameters[i]) {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public static bool IsGeneric<T>(this Type type) {
            return type.IsGeneric(typeof(T));
        }

        public static bool Is<T>(this Type type) {
            return type.Is(typeof(T));
        }

        public static bool Is(this Type type, Type root) {
            return type != null && root != null &&
                (root.IsAssignableFrom(type) ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == root)
                );
        }

        public static object GetDefaultValue(this Type type) {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        public static bool HasConstructor(this Type type, params Type[] types) {
            return types == null || types.Length == 0
                ? type.HasDefaultConstructor()
                : type.GetConstructor(types) != null;
        }

        public static bool HasDefaultConstructor(this Type type) {
            return type.GetConstructor(Type.EmptyTypes) != null;
        }

        /// <summary>
        /// Receive all properties for each class in lineage.
        /// </summary>
        public static IReadOnlyList<PropertyInfo> GetAllProperties(this Type type, BindingFlags flags) {
            var members = new List<PropertyInfo>();
            members.AddRange(type.GetProperties(flags));
            if (type.BaseType != null) {
                members.AddRange(type.BaseType.GetAllProperties(flags));
            }
            return members;
        }

        /// <summary>
        /// Receive all fields for each class in lineage.
        /// </summary>
        public static IReadOnlyList<FieldInfo> GetAllFields(this Type type, BindingFlags flags) {
            var members = new List<FieldInfo>();
            members.AddRange(type.GetFields(flags));
            if (type.BaseType != null) {
                members.AddRange(type.BaseType.GetAllFields(flags));
            }
            return members;
        }

        /// <summary>
        /// Receive top properties (static, instance, public and non public) from class hierarchy
        /// </summary>
        public static IReadOnlyList<PropertyInfo> GetTopProperties(this Type type) {
            return type.GetTopProperties(
                BindingFlags.Static |
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.NonPublic
                );
        }

        /// <summary>
        /// Receive top properties from class hierarchy.
        /// </summary>
        public static IReadOnlyList<PropertyInfo> GetTopProperties(this Type type, BindingFlags bindings) {
            var properties = new List<PropertyInfo>();
            while (type != null) {
                foreach (var property in type.GetProperties(bindings)) {
                    if (!properties.Any(item => item.Name == property.Name)) {
                        properties.Add(property);
                    }
                }
                type = type.BaseType;
            }
            return properties;
        }


        /// <summary>
        /// Receive top fields from class hierarchy.
        /// </summary>
        public static IReadOnlyList<FieldInfo> GetTopFields(this Type type, BindingFlags bindings) {
            var fields = new List<FieldInfo>();
            while (type != null) {
                foreach (var field in type.GetFields(bindings)) {
                    if (!fields.Any(item => item.Name == field.Name)) {
                        fields.Add(field);
                    }
                }
                type = type.BaseType;
            }
            return fields;
        }

        public static IReadOnlyList<Type> GetLineage(this Type type) {
            var lineage = new List<Type>();
            while (type != null) {
                lineage.Add(type);
                type = type.BaseType;
            }
            return lineage;
        }

        public static IReadOnlyList<string> GetNamespaces(this Type type) {
            return type.Namespace.Split('.');
        }
    }
}
