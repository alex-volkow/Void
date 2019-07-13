using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void
{
    /// <summary>
    /// Extends base functional of Random.
    /// </summary>
    public sealed class RandomGenerator : Random
    {
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
    }
}
