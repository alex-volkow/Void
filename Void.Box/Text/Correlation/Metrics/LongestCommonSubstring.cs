using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Void.Text
{
    public class LongestCommonSubstring : Metric
    {
        public override double Correlate(string source, string target) {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target)) {
                return default(double);
            }
            var substring = Calculate(source, target);
            var lowbounds = (double)Math.Min(source.Length, target.Length);
            return substring.Length / lowbounds;
        }

        public string Calculate(string source, string target) {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target)) {
                return string.Empty;
            }
            var matrix = new int[source.Length, target.Length];
            var maximumLength = 0;
            var lastSubsBegin = 0;
            var buffer = new StringBuilder();
            for (int i = 0; i < source.Length; i++) {
                for (int j = 0; j < target.Length; j++) {
                    if (source[i] != target[j]) {
                        matrix[i, j] = 0;
                        continue;
                    }
                    if (i == 0 || j == 0) {
                        matrix[i, j] = 1;
                    }
                    else {
                        matrix[i, j] = 1 + matrix[i - 1, j - 1];
                    }
                    if (matrix[i, j] > maximumLength) {
                        maximumLength = matrix[i, j];
                        var thisSubsBegin = i - matrix[i, j] + 1;
                        if (lastSubsBegin == thisSubsBegin) {
                            buffer.Append(source[i]);
                        }
                        else {
                            lastSubsBegin = thisSubsBegin;
                            buffer.Length = 0;
                            buffer.Append(source.Substring(
                                lastSubsBegin,
                                (i + 1) - lastSubsBegin)
                                );
                        }
                    }
                }
            }
            return buffer.ToString();
        }
    }
}
