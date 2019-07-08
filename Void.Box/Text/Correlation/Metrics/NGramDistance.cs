using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Void.Text
{
    public class NGramDistance : Metric
    {
        public static int DefaultSize { get; } = 2;



        public int Size { get; }
        


        public NGramDistance() : this(DefaultSize) { }
        
        public NGramDistance(int size) {
            this.Size = size;
        }



        public override double Score(string source, string target) {
            return NGramDistance.Calculate(source, target, this.Size);
        }

        public override bool Equals(object obj) {
            return base.Equals(obj) && this.Size == ((NGramDistance)obj).Size;
        }

        public override int GetHashCode() {
            var a = base.GetHashCode();
            var b = this.Size;
            return (a - b) * (a + b);
        }

        public static double Calculate(string source, string target, int size) {
            if (source == null || target == null) {
                return default(double);
            }
            if (source.Length == 0 || target.Length == 0) {
                return source.Length == target.Length ? 1.0 : 0.0;
            }
            var cost = default(int);
            if (source.Length < size || target.Length < size) {
                var min = Math.Min(source.Length, target.Length);
                var max = Math.Max(source.Length, target.Length);
                for (var k = 0; k < min; k++) {
                    if (source[k] == target[k]) {
                        cost++;
                    }
                }
                return (double)cost / max;
            }
            var sa = new char[source.Length + size - 1];
            var previousCosts = default(double[]);
            var horizontalCosts = default(double[]);
            var tempSwapper = default(double[]);
            for (var k = 0; k < sa.Length; k++) {
                if (k < size - 1) {
                    sa[k] = (char)0;
                } 
                else {
                    sa[k] = source[k - size + 1];
                }
            }
            previousCosts = new double[source.Length + 1];
            horizontalCosts = new double[source.Length + 1];
            var sourceIterator = default(int);
            var targetIterator = default(int);
            var targetNgrams = new char[size];
            for (sourceIterator = 0; sourceIterator <= source.Length; sourceIterator++) {
                previousCosts[sourceIterator] = sourceIterator;
            }
            for (targetIterator = 1; targetIterator <= target.Length; targetIterator++) {
                if (targetIterator < size) {
                    for (int ti = 0; ti < size - targetIterator; ti++) {
                        targetNgrams[ti] = '\0';
                    }
                    for (int ti = size - targetIterator; ti < size; ti++) {
                        targetNgrams[ti] = target[ti - (size - targetIterator)];
                    }
                }
                else {
                    targetNgrams = target.Substring(targetIterator - size, size).ToCharArray();
                }
                horizontalCosts[0] = targetIterator;
                for (sourceIterator = 1; sourceIterator <= source.Length; sourceIterator++) {
                    cost = 0;
                    int tn = size;
                    for (int ni = 0; ni < size; ni++) {
                        if (sa[sourceIterator - 1 + ni] != targetNgrams[ni]) { cost++; }
                        else if (sa[sourceIterator - 1 + ni] == 0) { tn--; }
                    }
                    var ec = (double)cost / tn;
                    horizontalCosts[sourceIterator] = Math.Min(Math.Min(horizontalCosts[sourceIterator - 1] + 1, previousCosts[sourceIterator] + 1), previousCosts[sourceIterator - 1] + ec);
                }
                tempSwapper = previousCosts;
                previousCosts = horizontalCosts;
                horizontalCosts = tempSwapper;
            }
            return 1.0D - ((double)previousCosts[source.Length] / Math.Max(target.Length, source.Length));
        }
    }
}
