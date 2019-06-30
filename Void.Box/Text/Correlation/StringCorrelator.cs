﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Void.Text.Correlation
{
    public class StringCorrelator
    {
        public bool IgnoreSpaces { get; set; }

        public bool CaseSensitive { get; set; }

        public bool ParallelProcessing { get; set; }

        public Condition Condition { get; set; }

        public ISet<Metric> Metrics { get; }



        public StringCorrelator() {
            this.Metrics = new HashSet<Metric>();
            this.CaseSensitive = false;
            this.IgnoreSpaces = false;
            this.Condition = Condition.Average;
        }



        public double Correlate(string source, string target) {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target)) {
                return default(double);
            }
            if (!this.CaseSensitive) {
                source = source.ToLower();
                target = target.ToLower();
            }
            if (this.IgnoreSpaces) {
                source = source.RemoveWhiteSpaces();
                target = target.RemoveWhiteSpaces();
                //source = Regex.Replace(source, "\\s", string.Empty);
                //target = Regex.Replace(target, "\\s", string.Empty);
            }
            //var results = new List<double>(this.Metrics.Count);
            //foreach (var metric in this.Metrics) results.Add(
            //    metric.Score(source, target)
            //    );
            var results = this.Metrics
                .Where(e => e != null)
                .Select(e => e.Score(source, target));
            switch (this.Condition) {
                case Condition.Maximum: return results.Max();
                case Condition.Minimum: return results.Min();
                default: return results.Average();
            }
        }

        private IEnumerable<double> Process(string source, string target) {
            if (this.ParallelProcessing) {
                return this.Metrics
                    .Where(e => e != null)
                    .AsParallel()
                    .Select(e => e.Score(source, target));
            }
            else {
                return this.Metrics
                    .Where(e => e != null)
                    .Select(e => e.Score(source, target));
            }
        }
    }
}
