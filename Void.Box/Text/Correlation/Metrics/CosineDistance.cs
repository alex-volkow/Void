using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
//using Fel.Collections;

namespace Void.Text.Correlation
{
    public class CosineDistance : Metric
    {
        private static readonly Regex TEMPLATE = new Regex(@"\w+");



        public override double Score(string source, string target) {
            return CosineDistance.Calculate(source, target);
        }



        public static double Calculate(string source, string target) {
            var sourceTokens = ExtractTokens(source);
            var targetTokens = ExtractTokens(target);
            var sourceVector = CollectVector(sourceTokens);
            var tergetVector = CollectVector(targetTokens);
            return CorrelateVectors(sourceVector, tergetVector);
        }

        private static IEnumerable<string> ExtractTokens(string text) {
            return !string.IsNullOrWhiteSpace(text)
                ? TEMPLATE.Matches(text).Cast<Match>().Select(e => e.Value)
                : new string[] { };
            //var tokens = new List<string>();
            //if (string.IsNullOrWhiteSpace(text)) return tokens;
            //foreach (Match match in TEMPLATE.Matches(text)) tokens.Add(match.Value);
            //return tokens;
        }

        private static Dictionary<string, int> CollectVector(IEnumerable<string> tokens) {
            var map = new Dictionary<string, int>();
            foreach (var token in tokens) {
                if (map.ContainsKey(token)) {
                    map[token] += 1;
                }
                else {
                    map.Add(token, 1);
                }
            }
            return map;
        }

        private static double CorrelateVectors(Dictionary<string, int> source, Dictionary<string, int> target) {
            var intersection = new HashSet<string>(source.Keys.Intersect(target.Keys));
            var product = default(long);
            foreach (var key in intersection) {
                product += source[key] * target[key];
            }
            var sourceDistance = default(double);
            foreach (int value in source.Values) {
                sourceDistance += Math.Pow(value, 2);
            }
            if (sourceDistance <= 0.0) {
                return 0.0;
            }
            var targetDistance = default(double);
            foreach (int value in target.Values) {
                targetDistance += Math.Pow(value, 2);
            }
            if (targetDistance <= 0.0) {
                return 0.0;
            }
            return product / ((Math.Sqrt(sourceDistance) * Math.Sqrt(targetDistance)));
        }
    }
}
