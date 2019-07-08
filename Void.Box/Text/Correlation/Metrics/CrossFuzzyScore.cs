using System;
using System.Globalization;

namespace Void.Text
{
    public class CrossFuzzyScore : FuzzyScore
    {
        public CrossFuzzyScore(CultureInfo culture) : base(culture) { }

        public CrossFuzzyScore() : base(CultureInfo.CurrentCulture) { }


        public override double Score(string source, string target) {
            return Math.Max(
                base.Score(source, target),
                base.Score(target, source)
            );
        }
    }
}
