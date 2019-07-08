using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Void.Text
{
    public class LevenshteinDistance : Metric
    {
        public override double Correlate(string source, string target) {
            var distance = LevenshteinDistance.Calculate(source, target);
            var upbounds = Math.Max(source?.Length ?? 0, target?.Length ?? 0);
            return 1 - distance / (upbounds != 0 ? (double)upbounds : 1.0);
        }

        public static int Calculate(string source, string target) {
            if (source.Length == 0) {
                return target.Length;
            }
            if (target.Length == 0) {
                return source.Length;
            }
            var distances = new int[source.Length + 1, target.Length + 1];
            for (int i = 0; i <= source.Length; distances[i, 0] = i++);
            for (int j = 0; j <= target.Length; distances[0, j] = j++);
            for (int i = 1; i <= source.Length; i++) {
                for (int j = 1; j <= target.Length; j++) {
                    var cost = target[j - 1] == source[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min(
                            Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                            distances[i - 1, j - 1] + cost
                        );
                }
            }
            return distances[source.Length, target.Length];
        }
    }
}
