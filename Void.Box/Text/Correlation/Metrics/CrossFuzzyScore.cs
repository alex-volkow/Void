using System;
using System.Globalization;

namespace Void.Text
{
    public class CrossFuzzyScore : FuzzyScore
    {
        public CrossFuzzyScore(CultureInfo culture) : base(culture) { }

        public CrossFuzzyScore() : base(CultureInfo.CurrentCulture) { }


        public override double Correlate(string source, string target) {
            return Math.Max(
                base.Correlate(source, target),
                base.Correlate(target, source)
            );
        }
    }
}
