﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Void.Text
{
    public class JaccardDistance : Metric
    {
        public override double Correlate(string source, string target) {
            return JaccardDistance.Calculate(source, target);
        }

        public static double Calculate(string source, string target) {
            if (source.Length == 0 || target.Length == 0) {
                return default(double);
            }
            var intersect = (double)source.Intersect(target).Count();
            var union = (double)source.Union(target).Count();
            return intersect / union;
        }
    }
}
