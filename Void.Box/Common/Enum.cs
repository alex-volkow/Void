using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Void.Common
{
    public static class Enums
    {
        public static T Get<T>(this Enum item) where T : Attribute {
            return GetAttributes<T>(item).First();
        }

        public static IReadOnlyList<T> GetAll<T>(this Enum item) where T : Attribute {
            return GetAttributes<T>(item);
        }

        private static IReadOnlyList<A> GetAttributes<A>(Enum value) where A : Attribute {
            var type = value.GetType();
            return type
                .GetField(value.ToString())
                .GetCustomAttributes<A>()
                .ToArray();
        }

        private static A GetAttribute<A>(Enum value) where A : Attribute {
            return GetAttributes<A>(value).FirstOrDefault();
        }
    }

    public class Enum<T> where T : struct
    {
        private static readonly IReadOnlyList<T> values;
        private static readonly IReadOnlyList<string> names;


        public static IReadOnlyList<string> Names {
            get { return Enum<T>.names; }
        }

        public static IReadOnlyList<T> Values {
            get { return Enum<T>.values; }
        }


        static Enum() {
            if (!typeof(T).IsEnum) {
                throw new InvalidOperationException(
                    String.Format("Type \"{0}\" is not enum", typeof(T).FullName)
                );
            }
            Enum<T>.values = new List<T>(
                (IEnumerable<T>)Enum.GetValues(typeof(T))
            );
            Enum<T>.names = new List<string>(
                (IEnumerable<string>)Enum.GetNames(typeof(T))
            );
        }


        protected Enum() { }


        public static IReadOnlyList<A> GetAttributes<A>(T value) where A : Attribute {
            return (value as Enum).GetAll<A>();
        }

        public static A GetAttribute<A>(T value) where A : Attribute {
            return GetAttributes<A>(value).FirstOrDefault();
        }

        public static string GetDescription(T value) {
            var attibute = GetAttribute<DescriptionAttribute>(value);
            return attibute?.Description;
        }

        public static T Parse(string text) {
            return Enum<T>.Parse(text, false);
        }

        public static T Parse(string text, bool ignoreCase) {
            return (T)Enum.Parse(typeof(T), text, ignoreCase);
        }

        public static IReadOnlyList<T> Parse(IEnumerable<string> items) {
            return Enum<T>.Parse(items, false);
        }

        public static IReadOnlyList<T> Parse(IEnumerable<string> items, bool ignoreCase) {
            var list = new List<T>(items.Count());
            foreach (var item in items) list.Add(Enum<T>.Parse(item, ignoreCase));
            return list;
        }

        public static T? TryParse(string text) {
            return Enum<T>.TryParse(text, false);
        }

        public static T? TryParse(string text, bool ignoreCase) {
            if (text == null) {
                return null;
            }
            return Enum.TryParse(text, ignoreCase, out T value)
                ? new T?(value)
                : null;
        }
    }
}
