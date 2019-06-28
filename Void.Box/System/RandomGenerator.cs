using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void
{
    public sealed class RandomGenerator : Random
    {
        public static RandomGenerator Default { get; } = new RandomGenerator((int)(DateTime.Now.Ticks >> 32));


        public RandomGenerator() {
        }

        public RandomGenerator(int seed)
            : base(seed) {
        }



        public bool NextBool() {
            return Next(2) == 0;
        }

        public int NextIntSign() {
            return NextBool() ? 1 : -1;
        }

        public double NextDoubleSign() {
            return NextBool() ? 1.0 : -1.0;
        }

        public char NextChar(params char[] chars) {
            return NextChar((IEnumerable<char>)chars);
        }

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
