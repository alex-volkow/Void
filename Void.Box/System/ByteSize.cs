using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Void
{
    /// <summary>
    /// Represents a byte count with a unit.
    /// </summary>
    public struct ByteSize : IEquatable<ByteSize>, IEquatable<decimal>, IEquatable<long>, IEquatable<int>, ICloneable, IComparable<ByteSize>
    {
        /// <summary>
        /// Get bytes count.
        /// </summary>
        public decimal Value { get; }


        /// <summary>
        /// Get bytes count in specific unit.
        /// </summary>
        /// <param name="unit">Bytes unit.</param>
        /// <returns>Bytes count.</returns>
        public decimal this[ByteUnit unit] {
            get {
                return this.Value / unit.GetFactor();
            }
        }


        /// <summary>
        /// Initialize a new instance with a count of bytes.
        /// </summary>
        /// <param name="value">Bytes count.</param>
        public ByteSize(decimal value) {
            this.Value = Math.Abs(value);
        }

        /// <summary>
        /// Initialize a new instance with a count of bytes in specific unit.
        /// </summary>
        /// <param name="value">Bytes count.</param>
        /// <param name="unit">Bytes unit.</param>
        public ByteSize(decimal value, ByteUnit unit)
            : this(ToBytes(value, unit)) {
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

        public bool Equals(decimal other) {
            return other == this.Value;
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
            if (obj is decimal decimalValue) {
                return Equals(decimalValue);
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

        /// <summary>
        /// Convert a count of bytes in specitic unit to bytes.
        /// </summary>
        /// <param name="value">Bytes count.</param>
        /// <param name="unit">Bytes unit.</param>
        /// <returns>Bytes count.</returns>
        public static decimal ToBytes(decimal value, ByteUnit unit) {
            return value * unit.GetFactor();
        }

        /// <summary>
        /// Parse a input string to ByteSize.
        /// </summary>
        /// <param name="text">Text representation of ByteSize</param>
        /// <returns>ByteSize instance</returns>
        /// <exception cref="FormatException">Input string in invalid format.</exception>
        public static ByteSize Parse(string text) {
            return TryParse(text, out ByteSize value) ? value : throw new FormatException();
        }

        /// <summary>
        /// Try to parse a input string to ByteSize
        /// </summary>
        /// <param name="text">Text representation of ByteSize</param>
        /// <param name="value">Prsing result</param>
        /// <returns>True if success else False</returns>
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
                if (Char.IsDigit(symbol)) {
                    number.Add(symbol);
                }
                if (symbol == '.' || symbol == ',') {
                    number.Add('.');
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
                        value = new ByteSize(val, unitValue.Value);
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
