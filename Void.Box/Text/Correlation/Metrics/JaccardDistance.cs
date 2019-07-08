using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Void.Text
{
    public class JaccardDistance : Metric
    {
        public override double Correlate(string source, string target) {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target)) {
                return default(double);
            }
            var intersect = (double)source.Intersect(target).Count();
            var union = (double)source.Union(target).Count();
            return intersect / union;
        }
    }
}
