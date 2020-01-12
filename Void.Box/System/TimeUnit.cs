using System;
using System.Collections.Generic;
using System.Text;

namespace Void
{
    public enum TimeUnit
    {
        [TimeUnit("ns", 1)]
        Nanoseconds,

        [TimeUnit("us", 1L * 1000L)]
        Microsecond,

        [TimeUnit("ms", 1L * 1000L * 1000L)]
        Millisecond,

        [TimeUnit("s", 1L * 1000L * 1000L * 1000L)]
        Second,

        [TimeUnit("m", 60L * 1000L * 1000L * 1000L)]
        Minute,

        [TimeUnit("h", 60L * 60L * 1000L * 1000L * 1000L)]
        Hour,

        [TimeUnit("d", 24L * 60L * 60L * 1000L * 1000L * 1000L)]
        Day,

        [TimeUnit("w", 7L * 24L * 60L * 60L * 1000L * 1000L * 1000L)]
        Week
    }

    internal class TimeUnitAttribute : Attribute
    {
        public long Factor { get; }

        public string Abbreviation { get; }

        public TimeUnitAttribute(string abbreviation, long factor) {
            this.Abbreviation = abbreviation;
            this.Factor = factor;
        }
    }

    internal static class TimeUnitExtensions
    {
        public static TimeUnitAttribute GetAttribute(this TimeUnit value) {
            return Enum<TimeUnit>.GetAttribute<TimeUnitAttribute>(value);
        }
    }
}
