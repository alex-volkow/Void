using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Void.Text
{
    public class JaroDistance : JaroWinklerDistance
    {
        public override double Correlate(string source, string target) {
            return Correlate(source, target, 0D);
        }
    }
}
