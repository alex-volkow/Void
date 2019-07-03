using Void.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.IO
{
    public class EntryMap<T> : Map<string, T>, IEntryMap<T> where T : class, IEntryInfo
    {
        IEnumerable<string> IEntryMap<T>.Keys => this.Keys;
        IEnumerable<T> IEntryMap<T>.Values => this.Values;

        public void Add(T entry) {
            Add(entry?.Path, entry);
        }
    }
}
