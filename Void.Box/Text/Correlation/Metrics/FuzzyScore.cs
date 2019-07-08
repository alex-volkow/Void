using System;
using System.Globalization;

namespace Void.Text
{
    public class FuzzyScore : Metric
    {
        public CultureInfo Culture { get; }



        public FuzzyScore() : this(CultureInfo.CurrentCulture) { }

        public FuzzyScore(CultureInfo culture) {
            this.Culture = culture ?? throw new NullReferenceException(nameof(culture));
        }

        

        public override double Correlate(string source, string target) {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target)) {
                return default(double);
            }
            var bound = Math.Min(source.Length, target.Length);
            var score = Calculate(source, target);
            return score / (bound + (bound - 1) * 2.0);
        }

        public int Calculate(string source, string target) {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target)) {
                return default(int);
            }
            var score = default(int);
            int previous = int.MinValue;
            for (int i = 0, j = 0; j < target.Length; j++) {
                var found = false;
                for (; i < source.Length && !found; i++) {
                    if (target[j] == source[i]) {
                        score++;
                        if (previous + 1 == i) {
                            score += 2;
                        }
                        previous = i;
                        found = true;
                    }
                }
            }
            return score;
        }
    }
}
