using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Void
{
    public struct ByteSize : IEquatable<ByteSize>, IEquatable<long>, IEquatable<int>, ICloneable, IComparable<ByteSize>
    {
        //private static readonly Regex parser = new Regex(@"^[\t ]*(?<VALUE>(\d+([\.,]\d+)?)|([\.,]\d+))[\t ]*(?<UNIT>\p{L}+)?[\t ]*$");



        public long Value { get; }



        public double this[ByteUnit unit] {
            get {
                return this.Value / unit.GetFactor();
            }
        }



        public ByteSize(long value) {
            this.Value = Math.Abs(value);
        }

        public ByteSize(long value, ByteUnit unit)
            : this(ToBytes(value, unit)) {
        }

        public ByteSize(double value, ByteUnit unit)
            : this((long)value, unit) {
        }



        public int CompareTo(ByteSize other) {
            return this.Value.CompareTo(other.Value);
        }

        public object Clone() {
            return new ByteSize(this.Value);
        }

        public bool Equals(ByteSize other) {
            return other.Value == this.Value;
        }

        public bool Equals(long other) {
            return other == this.Value;
        }

        public bool Equals(int other) {
            return other == this.Value;
        }

        public override bool Equals(object obj) {
            if (obj is ByteSize otherByteSize) {
                return Equals(otherByteSize);
            }
            if (obj is long otherLong) {
                return Equals(otherLong);
            }
            if (obj is int otherInt) {
                return Equals(otherInt);
            }
            return false;
        }

        public override int GetHashCode() {
            return this.Value.GetHashCode();
        }

        public override string ToString() {
            var units = Enum<ByteUnit>.Values
                .Select(item => new { Value = item, Factor = item.GetFactor() })
                .OrderByDescending(item => item.Factor)
                .ToArray();
            for (var i = 0; i < units.Length; i++) {
                var mod = (int)(this[units[i].Value] % units[i].Factor);
                if (mod > 0) {
                    return ToString(units[i].Value, true);
                }
            }
            return ToString(ByteUnit.None, true);
        }

        public string ToString(ByteUnit unit, int decimals) {
            return ToString(unit, decimals, true);
        }

        public string ToString(ByteUnit unit, bool space = true) {
            return ToString(unit, 2, space);
        }

        public string ToString(ByteUnit unit, int decimals, bool space = true) {
            var separator = space ? " " : string.Empty;
            var suffix = unit.GetInfo().Suffix;
            var value = this[unit].ToString($"N{decimals}");
            return $"{value}{separator}{suffix}";
        }

        public static long ToBytes(double value, ByteUnit unit) {
            return (long)(value * unit.GetFactor());
        }

        public static ByteSize Parse(string text) {
            return TryParse(text, out ByteSize value) ? value : throw new FormatException();
        }

        public static bool TryParse(string text, out ByteSize value) {
            value = default(ByteSize);
            if (string.IsNullOrWhiteSpace(text)) {
                return false;
            }
            var chars = text.Trim().ToLower().ToCharArray();
            var number = new List<char>(chars.Length);
            var unit = new List<char>(chars.Length);
            if (chars.Count(e => e == '.' || e == ',') > 1) {
                return false;
            }
            var index = 0;
            while (index < chars.Length) {
                var symbol = chars[index++];
                if (Char.IsDigit(symbol) || symbol == '.' || symbol == ',') {
                    number.Add(symbol);
                }
                if (Char.IsWhiteSpace(symbol) || Char.IsLetter(symbol)) {
                    index--;
                    break;
                }
            }
            if (Char.IsPunctuation(number[number.Count - 1])) {
                number.RemoveAt(number.Count - 1);
            }
            while (index < chars.Length) {
                var symbol = chars[index++];
                if (Char.IsWhiteSpace(symbol)) {
                    continue;
                }
                if (Char.IsLetter(symbol)) {
                    unit.Add(symbol);
                    continue;
                }
                return false;
            }
            var numberText = new string(number.ToArray());
            if (decimal.TryParse(numberText, out decimal val)) {
                if (unit.Count > 0) {
                    var unitValue = ParseUnit(new string(unit.ToArray()));
                    if (unitValue != null) {
                        value = new ByteSize((double)val, unitValue.Value);
                        return true;
                    }
                }
                else {
                    value = new ByteSize((long)val);
                    return true;
                }
            }
            return false;
        }

        private static ByteUnit? ParseUnit(string value) {
            foreach (var unit in Enum<ByteUnit>.Values) {
                var suffixes = unit.GetSuffixes();
                var comparer = suffixes.IgnoreCase
                    ? StringComparison.InvariantCultureIgnoreCase
                    : StringComparison.InvariantCulture;
                foreach (var suffix in suffixes.Values) {
                    if (suffix.Equals(value, comparer)) {
                        return unit;
                    }
                }
            }
            return null;
        }
    }
}
