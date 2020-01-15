using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Void
{
    public struct TimeInterval : IEquatable<TimeInterval>, IEquatable<TimeSpan>, IComparable<TimeInterval>, ICloneable
    {
        private const decimal TICKS_PER_NANOSECOND = TimeSpan.TicksPerMillisecond / 1000000M;

        private readonly decimal nanoseconds;



        public decimal this[TimeUnit unit] {
            get {
                var attribute = unit.GetAttribute();
                return this.nanoseconds / attribute.Factor;
            }
        }



        private TimeInterval(decimal value) {
            this.nanoseconds = value;
        }

        public TimeInterval(TimeSpan timespan) {
            this.nanoseconds = TicksToNanoseconds(timespan.Ticks);
        }

        public TimeInterval(decimal value, TimeUnit unit)
            : this(ToNanoseconds(value, unit)) {
        }



        public static TimeInterval Parse(string text) {
            return TryParse(text) ?? throw new FormatException(
                "Invalid string format"
                );
        }

        public static TimeInterval? TryParse(string text) {
            if (string.IsNullOrWhiteSpace(text)) {
                return default;
            }
            var number = new List<char>(text.Length);
            var abbreviation = new List<char>(text.Length);
            var isLetter = false;
            foreach (var c in text) {
                if (!isLetter && char.IsLetter(c)) {
                    isLetter = true;
                }
                if (isLetter) {
                    abbreviation.Add(c);
                }
                else {
                    number.Add(c);
                }
            }
            if (!decimal.TryParse(new string(number.ToArray()), out var numberValue)) {
                return default;
            }
            var abbreviationValue = new string(abbreviation.ToArray()).Trim();
            foreach (var unit in Enum<TimeUnit>.Values) {
                var attribute = unit.GetAttribute();
                if (attribute.Abbreviation.Equals(abbreviationValue, StringComparison.OrdinalIgnoreCase)) {
                    return new TimeInterval(numberValue, unit);
                }
            }
            return default;
        }

        public static bool TryParse(string text, out TimeInterval value) {
            var result = TryParse(text);
            value = result ?? default;
            return result.HasValue;
        }

        public object Clone() {
            return new TimeInterval(this.nanoseconds, TimeUnit.Nanoseconds);
        }

        public int CompareTo(TimeInterval other) {
            return this.nanoseconds.CompareTo(other.nanoseconds);
        }

        public bool Equals(TimeSpan other) {
            return ToTicks() == other.Ticks;
        }

        public bool Equals(TimeInterval other) {
            return this.nanoseconds == other.nanoseconds;
        }

        public override bool Equals(object obj) {
            if (obj is TimeUnit timeunit) {
                return Equals(timeunit);
            }
            if (obj is TimeSpan timespan) {
                return Equals(timespan);
            }
            return false;
        }

        public override int GetHashCode() {
            return this.nanoseconds.GetHashCode();
        }

        public override string ToString() {
            var value = Round();
            return $"{value.Item1:F2}{value.Item3}";
        }

        public string ToString(string format) {
            var value = Round();
            return $"{value.Item1.ToString(format)}{value.Item3}";
        }

        public string ToString(string format, TimeUnit unit) {
            var attribute = unit.GetAttribute();
            var value = this.nanoseconds / attribute.Factor;
            var number = format != null ? value.ToString(format) : value.ToString();
            return $"{number}{attribute.Abbreviation}";
        }

        public long ToTicks() {
            unchecked {
                return (long)(this.nanoseconds * TICKS_PER_NANOSECOND);
            }
        }

        private (decimal, TimeUnit, string) Round() {
            foreach (var unit in Enum<TimeUnit>.Values.OrderByDescending(e => (int)e)) {
                var attribute = unit.GetAttribute();
                if (Math.Abs(this.nanoseconds) >= attribute.Factor) {
                    return (this.nanoseconds / attribute.Factor, unit, attribute.Abbreviation);
                }
            }
            return (this.nanoseconds, TimeUnit.Nanoseconds, "ns");
        }

        private static decimal ToNanoseconds(decimal value, TimeUnit unit) {
            return value * unit.GetAttribute().Factor;
        }

        private static decimal TicksToNanoseconds(long ticks) {
            return ticks / TICKS_PER_NANOSECOND;
        }

        public static implicit operator TimeSpan(TimeInterval value) {
            return new TimeSpan(value.ToTicks());
        }

        public static implicit operator TimeInterval(TimeSpan value) {
            return new TimeInterval(value);
        }

        public static TimeInterval operator -(TimeInterval t) {
            return new TimeInterval(-t.nanoseconds);
        }

        public static TimeInterval operator -(TimeInterval t1, TimeInterval t2) {
            return new TimeInterval(t1.nanoseconds - t2.nanoseconds);
        }

        public static TimeInterval operator +(TimeInterval t) {
            return t;
        }

        public static TimeInterval operator +(TimeInterval t1, TimeInterval t2) {
            return new TimeInterval(t1.nanoseconds + t2.nanoseconds);
        }

        public static bool operator ==(TimeInterval t1, TimeInterval t2) {
            return t1.nanoseconds == t2.nanoseconds;
        }

        public static bool operator !=(TimeInterval t1, TimeInterval t2) {
            return t1.nanoseconds != t2.nanoseconds;
        }

        public static bool operator <(TimeInterval t1, TimeInterval t2) {
            return t1.nanoseconds < t2.nanoseconds;
        }

        public static bool operator <=(TimeInterval t1, TimeInterval t2) {
            return t1.nanoseconds <= t2.nanoseconds;
        }

        public static bool operator >(TimeInterval t1, TimeInterval t2) {
            return t1.nanoseconds > t2.nanoseconds;
        }

        public static bool operator >=(TimeInterval t1, TimeInterval t2) {
            return t1.nanoseconds >= t2.nanoseconds;
        }
    }
}
