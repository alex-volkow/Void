using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Void
{
    [Parallelizable]
    public class TimeIntervalTests
    {
        [Test]
        public void Converting() {
            var matches = new Dictionary<TimeInterval, long> { 
                [new TimeInterval(100, TimeUnit.Millisecond)] = 100,
                [new TimeInterval(3.5M, TimeUnit.Minute)] = (60 *3 + 30) * 1000,
                [new TimeInterval(2, TimeUnit.Hour)] = 2 * 60 * 60 * 1000
            };
            foreach (var match in matches) {
                var origin = TimeSpan.FromMilliseconds(match.Value);
                Assert.True(origin == (TimeSpan)match.Key, $"{origin} != {match.Key}");
            }
        }

        [Test]
        public void Parsing() {
            var matches = new Dictionary<string, long> {
                ["100ms"] = 100,
                ["3.5m"] = (60 * 3 + 30) * 1000,
                ["2h"] = 2 * 60 * 60 * 1000
            };
            foreach (var match in matches) {
                var origin = TimeSpan.FromMilliseconds(match.Value);
                if (!TimeInterval.TryParse(match.Key, out var value)) {
                    Assert.Fail($"Failed to parse: {match.Key}");
                }
                Assert.True(origin == (TimeSpan)value, $"{origin} != {match.Key}");
            }
        }
    }
}
