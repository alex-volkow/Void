using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Void.Text.Correlation
{
    public class JaroDistance : Metric
    {
        public override double Score(string source, string target) {
            return JaroDistance.Calculate(source, target);
        }

        public static double Calculate(string source, string target) {
            return JaroWinklerDistance.Calculate(source, target, 0D);
        }
    }
}
