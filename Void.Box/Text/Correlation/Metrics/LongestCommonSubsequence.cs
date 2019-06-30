using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Void.Text.Correlation
{
    public class LongestCommonSubsequence : Metric
    {
        public override double Score(string source, string target) {
            if (source == null || target == null || source.Length == 0 || target.Length == 0) {
                return default(double);
            }
            var subsequence = LongestCommonSubsequence.Calculate(source, target);
            var lowerbounds = (double)Math.Min(source.Length, target.Length);
            return subsequence.Length / lowerbounds;
        }

        public static string Calculate(string source, string target) {
            if (source == null || target == null || source.Length == 0 || target.Length == 0) {
                return string.Empty;
            }
            var matrix = new int[source.Length + 1, target.Length + 1];
            for (int i = 0; i < source.Length + 1; i++) { matrix[i, 0] = 0; }
            for (int j = 0; j < target.Length + 1; j++) { matrix[0, j] = 0; }
            for (int i = 1; i < source.Length + 1; i++) {
                for (int j = 1; j < target.Length + 1; j++) {
                    matrix[i, j] = source[i - 1].Equals(target[j - 1])
                        ? matrix[i - 1, j - 1] + 1
                        : Math.Max(matrix[i, j - 1], matrix[i - 1, j]);
                }
            }
            return Backtrack(matrix, source, target, source.Length, target.Length);
        }

        private static string Backtrack(int[,] matrix, string source, string target, int i, int j) {
            if (i == 0 || j == 0) {
                return string.Empty;
            }
            if (source[i - 1].Equals(target[j - 1])) {
                return Backtrack(matrix, source, target, i - 1, j - 1) + source[i - 1];
            }
            return matrix[i, j - 1] > matrix[i - 1, j]
                ? Backtrack(matrix, source, target, i, j - 1)
                : Backtrack(matrix, source, target, i - 1, j);
        }
    }
}
