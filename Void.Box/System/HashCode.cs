using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void
{
    public static class HashCode
    {
        private static readonly int init = 17;
        private static readonly int factor = 31;

        public static int Create(params object[] args) {
            if (args == null) {
                return default(int);
            }
            var hash = init;
            for (var i = 0; i < args.Length; i++) {
                if (args[i] != null) {
                    hash = hash * factor + args[i].GetHashCode();
                }
            }
            return hash;
        }

        public static int Create(IEnumerable args) {
            if (args == null) {
                return default(int);
            }
            var hash = init;
            foreach (var arg in args) {
                hash = hash * factor + arg?.GetHashCode() ?? default(int);
            }
            return hash;
        }
    }
}
