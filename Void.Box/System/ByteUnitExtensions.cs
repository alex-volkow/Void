using System;
using System.Collections.Generic;
using System.Text;

namespace Void
{
    internal static class ByteUnitExtensions
    {
        public static decimal GetFactor(this ByteUnit unit) {
            var power = unit.GetInfo().Power;
            var factor = 1M;
            if (power > 0) {
                for (var i = 0; i < power; i++) {
                    factor *= 1024;
                }
            }
            return factor;
        }

        public static UnitAttribute GetInfo(this ByteUnit unit) {
            return Enum<ByteUnit>.GetAttribute<UnitAttribute>(unit);
        }

        public static SuffixesAttribute GetSuffixes(this ByteUnit unit) {
            return Enum<ByteUnit>.GetAttribute<SuffixesAttribute>(unit);
        }
    }
}
