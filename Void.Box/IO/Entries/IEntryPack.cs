using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.IO
{
    public interface IEntryPack<out T> : IReadOnlyDictionary<string, T> where T : IEntryInfo { }
}
