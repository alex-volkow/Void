using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Void
{
    public static class StringExtensions
    {
        public static string Multiply(this string source, int value) {
            if (value < 0) {
                throw new IndexOutOfRangeException(
                    "Failed to multiply a string with a negative number"
                    );
            }
            if (string.IsNullOrEmpty(source) && value != 0) {
                return source;
            }
            if (value == 0) {
                return source != null
                    ? string.Empty
                    : source;
            }
            var text = new StringBuilder(source);
            for (var i = 0; i < (value - 1); i++) {
                text.Append(source);
            }
            return text.ToString();
        }

        public static string RemoveFirst(this string source, string substring) {
            return source?.ReplaceFirst(substring, string.Empty);
        }

        public static string RemoveLast(this string source, string substring) {
            return source?.ReplaceLast(substring, string.Empty);
        }

        public static string Remove(this string source, string substring) {
            return source?.Replace(substring, string.Empty);
        }

        public static string ReplaceFirst(this string self, string source, string target) {
            var position = self.IndexOf(source);
            return position < 0 ? self : string.Format("{0}{1}{2}",
                self.Substring(0, position),
                target,
                self.Substring(position + source.Length)
                );
        }

        public static string ReplaceLast(this string self, string source, string target) {
            var position = self.LastIndexOf(source);
            return position < 0 ? self : self
                .Remove(position, source.Length)
                .Insert(position, target);
        }
    }
}
