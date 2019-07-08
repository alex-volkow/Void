using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Void.Text
{
    public abstract class Metric
    {
        public abstract double Correlate(string source, string target);

        public virtual TimeSpan GetCost(string source, string target, int iterations = 100) {
            iterations = Math.Abs(iterations);
            iterations++;
            var timer = new Stopwatch();
            var lastSum = default(long);
            var currentSum = default(long);
            var index = default(int);
            try {
                for (index = 0; index < iterations; index++) {
                    timer.Restart();
                    Correlate(source, target);
                    lastSum = currentSum;
                    currentSum = checked(currentSum + timer.ElapsedTicks);
                }
            }
            catch (OverflowException) {
                currentSum = lastSum;
                index--;
            }
            finally {
                timer.Stop();
            }
            return new TimeSpan(currentSum / (index + 1));
        }

        public override bool Equals(object obj) {
            return obj?.GetType() == this.GetType();
        }

        public override int GetHashCode() {
            return this.GetType().GetHashCode();
        }

        public override string ToString() {
            return this.GetType().Name;
        }
    }
}
