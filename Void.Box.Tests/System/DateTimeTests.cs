using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Void
{
    [Parallelizable]
    public class DateTimeTests
    {
        [Test]
        public void TruncateMilliseconds() {
            var now = DateTime.Now;
            var rounded = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var normal = rounded.AddMilliseconds(100);
            Assert.AreEqual(rounded, normal.Truncate(TimeUnit.Millisecond));
        }

        [Test]
        public void TruncateSeconds() {
            var now = DateTime.Now;
            var rounded = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            var normal = rounded.AddSeconds(5);
            Assert.AreEqual(rounded, normal.Truncate(TimeUnit.Second));
        }

        [Test]
        public void TruncateMinutes() {
            var now = DateTime.Now;
            var rounded = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            var normal = rounded.AddMinutes(20);
            Assert.AreEqual(rounded, normal.Truncate(TimeUnit.Minute));
        }

        [Test]
        public void TruncateHours() {
            var now = DateTime.Now;
            var rounded = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var normal = rounded.AddHours(6);
            Assert.AreEqual(rounded, normal.Truncate(TimeUnit.Hour));
        }
    }
}
