using System;
using System.Collections.Generic;
using System.Text;

namespace Void
{
    public static class ByteUnitExtensions
    {
        public static int GetFactor(this ByteUnit unit) {
            var power = unit.GetInfo().Power;
            var factor = 1;
            if (power > 0) {
                for (var i = 0; i < power; i++) {
                    factor = factor * 1024;
                }
            }
            return factor;
        }

        internal static UnitAttribute GetInfo(this ByteUnit unit) {
            return Enum<ByteUnit>.GetAttribute<UnitAttribute>(unit);
        }

        internal static SuffixesAttribute GetSuffixes(this ByteUnit unit) {
            return Enum<ByteUnit>.GetAttribute<SuffixesAttribute>(unit);
        }
    }
}
