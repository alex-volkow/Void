using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Void.Text
{
    public class JaroWinklerDistance : Metric
    {
        public static double DefaultThreshold { get; } = 0.7D;

        /// <summary>
        /// Value of the threshold used for adding the Winkler bonus.
        /// Negative value get the Jaro distance. The default value is 0.7.
        /// </summary>
        public double Threshold { get; }



        public JaroWinklerDistance() : this(DefaultThreshold) { }

        public JaroWinklerDistance(double threshold) {
            this.Threshold = threshold;
        }
        


        public override double Correlate(string source, string target) {
            return Correlate(source, target, this.Threshold);
        }

        public double Correlate(string source, string target, double threshold) {
            if (source == null) {
                source = string.Empty;
            }
            if (target == null) {
                target = string.Empty;
            }
            var matches = Matches(source, target);
            var first = (double)matches.First();
            if (first == 0) {
                return 0f;
            }
            var j = ((first / source.Length + first / target.Length + (first - matches[1]) / first)) / 3;
            return j < threshold ? j : j + Math.Min(0.1D, 1D / matches[3]) * matches[2] * (1 - j);
        }

        public override bool Equals(object obj) {
            return base.Equals(obj) && this.Threshold == ((JaroWinklerDistance)obj).Threshold;
        }

        public override int GetHashCode() {
            var a = base.GetHashCode();
            var b = (int)this.Threshold;
            return (a - b) * (a + b);
        }

        private static int[] Matches(string source, string target) {
            var max = default(string);
            var min = default(string);
            if (source.Length > target.Length) {
                max = source;
                min = target;
            }
            else {
                max = target;
                min = source;
            }
            var range = Math.Max(max.Length / 2 - 1, 0);
            var matchIndexes = new int[min.Length];
            for (var i = 0; i < matchIndexes.Length; i++) {
                matchIndexes[i] = -1;
            }
            var matchFlags = new bool[max.Length];
            var matches = default(int);
            for (var mi = 0; mi < min.Length; mi++) {
                var c1 = min[mi];
                for (int xi = Math.Max(mi - range, 0),
                    xn = Math.Min(mi + range + 1, max.Length); xi < xn; xi++) {
                    if (matchFlags[xi] || c1 != max[xi]) {
                        continue;
                    }
                    matchIndexes[mi] = xi;
                    matchFlags[xi] = true;
                    matches++;
                    break;
                }
            }
            var ms1 = new char[matches];
            var ms2 = new char[matches];
            for (int i = 0, si = 0; i < min.Length; i++) {
                if (matchIndexes[i] != -1) {
                    ms1[si] = min[i];
                    si++;
                }
            }
            for (int i = 0, si = 0; i < max.Length; i++) {
                if (matchFlags[i]) {
                    ms2[si] = max[i];
                    si++;
                }
            }
            var transpositions = ms1.Where((t, mi) => t != ms2[mi]).Count();
            var prefix = 0;
            for (var mi = 0; mi < min.Length; mi++) {
                if (source[mi] == target[mi]) {
                    prefix++;
                }
                else {
                    break;
                }
            }
            return new int[] { matches, transpositions / 2, prefix, max.Length };
        }
    }
}
