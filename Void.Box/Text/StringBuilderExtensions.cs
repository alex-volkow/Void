using System;
using System.Collections.Generic;
using System.Text;

namespace Void
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder TrimEnd(this StringBuilder builder) {
            if (builder == null || builder.Length == 0) {
                return builder;
            }
            var i = builder.Length - 1;
            while (i >= 0) {
                if (!char.IsWhiteSpace(builder[--i])) {
                    break;
                }
            }
            if (i < builder.Length - 1) {
                builder.Length = i + 1;
            }
            return builder;
        }

        public static StringBuilder TrimEnd(this StringBuilder builder, char c) {
            if (builder == null || builder.Length == 0) {
                return builder;
            }
            var i = builder.Length - 1;
            while (i >= 0) {
                if (builder[--i] != c) {
                    break;
                }
            }
            if (i < builder.Length - 1) {
                builder.Length = i + 1;
            }
            return builder;
        }
    }
}
