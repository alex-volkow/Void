using System;
using System.Collections.Generic;
using System.Text;

namespace Void
{
    public static class DateTimeExtensions
    {
        public static DateTime Truncate(this DateTime timestamp, TimeUnit unit) {
            if (timestamp == DateTime.MinValue || timestamp == DateTime.MaxValue) { 
                return timestamp; 
            }
            var interval = new TimeInterval(1, (TimeUnit)((int)unit + 1));
            return timestamp.AddTicks(-(timestamp.Ticks % interval.ToTicks()));
        }
    }
}
