using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Void.Collections;

namespace Void
{
    /// <summary>
    /// Extends base functional of Random.
    /// </summary>
    public sealed class RandomGenerator : Random
    {
        private static readonly string PASSWORD_CHARS_ALPHA = "abcdefgijkmnopqrstwxyz";
        private static readonly string PASSWORD_CHARS_NUMERIC = "0123456789";
        private static readonly string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";


        public static RandomGenerator Default { get; } = new RandomGenerator((int)(DateTime.Now.Ticks >> 32));


        public RandomGenerator() {
        }

        public RandomGenerator(int seed)
            : base(seed) {
        }


        /// <summary>
        /// Receive random boolean value.
        /// </summary>
        public bool NextBool() {
            return Next(2) == 0;
        }

        /// <summary>
        /// Receive random integer +1 or -1.
        /// </summary>
        public int NextIntSign() {
            return NextBool() ? 1 : -1;
        }

        /// <summary>
        /// Receive random deoble +1.0 or -1.0.
        /// </summary>
        /// <returns></returns>
        public double NextDoubleSign() {
            return NextBool() ? 1.0 : -1.0;
        }

        /// <summary>
        /// Select random char from array.
        /// </summary>
        public char NextChar(params char[] chars) {
            return NextChar((IEnumerable<char>)chars);
        }

        /// <summary>
        /// Select random char from enumeration.
        /// </summary>
        public char NextChar(IEnumerable<char> chars) {
            if (chars == null || chars.Count() == 0) {
                throw new InvalidOperationException(
                    "No chars have been received"
                    );
            }
            var index = Next(chars.Count());
            return chars.ElementAt(index);
        }

        /// <summary>
        /// Generate a string with random chars from the set.
        /// </summary>
        /// <param name="length">String length.</param>
        /// <param name="characters">Characters set.</param>
        public string NextString(int length, params char[] characters) {
            return NextString(length, (IEnumerable<char>)characters);
        }

        /// <summary>
        /// Generate a string with random chars from the set.
        /// </summary>
        /// <param name="length">String length.</param>
        /// <param name="characters">Characters set.</param>
        public string NextString(int length, IEnumerable<char> characters) {
            if (length < 1 || characters == null) {
                return string.Empty;
            }
            var symbols = characters.Distinct().ToArray();
            var text = new StringBuilder();
            while (0 < length--) {
                var index = Next(symbols.Length);
                text.Append(symbols[index]);
            }
            return text.ToString();
        }

        /// <summary>
        /// Generate a random password string using alphanumeric and special chars.
        /// </summary>
        /// <param name="length">Password length.</param>
        public string NextPassword(int length) {
            return NextPassword(length, -1);
        }

        /// <summary>
        /// Generate a random password string using alphanumeric and special chars.
        /// </summary>
        /// <param name="length">Password length.</param>
        /// <param name="specialCharsCount">Special chars count in the password string.
        /// If less than zero, then a random number will be used.
        /// </param>
        public string NextPassword(int length, int specialCharsCount) {
            var text = new StringBuilder();
            var symbols = new List<char>(PASSWORD_CHARS_ALPHA);
            symbols.AddRange(PASSWORD_CHARS_ALPHA.Select(e => Char.ToUpper(e)));
            symbols.AddRange(PASSWORD_CHARS_NUMERIC);
            if (specialCharsCount < 0) {
                specialCharsCount = (int)(Next(length) / 2.5);
            }
            if (specialCharsCount > length) {
                specialCharsCount = length;
            }
            while (0 < length--) {
                if (0 < specialCharsCount--) {
                    var index = Next(PASSWORD_CHARS_SPECIAL.Length);
                    text.Append(PASSWORD_CHARS_SPECIAL[index]);
                }
                else {
                    var index = Next(symbols.Count);
                    text.Append(symbols[index]);
                }
            }
            return new string(text
                .ToString()
                .Shuffle()
                .ToArray()
                );
        }
    }
}
