using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.IO
{
    public interface IEntryMap<T> : IEntryPack<T>, IDictionary<string, T> where T : IEntryInfo { }
}
