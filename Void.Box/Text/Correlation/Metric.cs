using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Void.Text.Correlation
{
    public abstract class Metric
    {
        public abstract double Score(string source, string target);

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
