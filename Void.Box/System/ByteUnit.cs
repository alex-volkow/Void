using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Void
{
    public enum ByteUnit
    {
        [Unit(Power = 0, Suffix = "bytes")]
        [Suffixes("bytes", "B", "", IgnoreCase = true)]
        None,

        [Unit(Power = 1, Suffix = "KB")]
        [Suffixes("KB", "KiB")]
        Kilo,

        [Unit(Power = 2, Suffix = "MB")]
        [Suffixes("MB", "MiB")]
        Mega,

        [Unit(Power = 3, Suffix = "GB")]
        [Suffixes("GB", "GiB")]
        Giga,

        [Unit(Power = 4, Suffix = "TB")]
        [Suffixes("TB", "TiB")]
        Tera,

        [Unit(Power = 5, Suffix = "PB")]
        [Suffixes("PB", "PiB")]
        Peta,

        [Unit(Power = 6, Suffix = "EB")]
        [Suffixes("EB", "EiB")]
        Exa,

        [Unit(Power = 7, Suffix = "ZB")]
        [Suffixes("ZB", "ZiB")]
        Zetta,

        [Unit(Power = 8, Suffix = "YB")]
        [Suffixes("YB", "YiB")]
        Yotta
    }

    internal class UnitAttribute : Attribute
    {
        public int Power { get; set; }
        public string Suffix { get; set; }
    }

    internal class SuffixesAttribute : Attribute
    {
        public IReadOnlyList<string> Values { get; private set; }

        public bool IgnoreCase { get; set; }

        public SuffixesAttribute(params string[] values) {
            this.Values = values.ToArray() ?? new string[] { };
            this.IgnoreCase = true;
        }
    }
}
