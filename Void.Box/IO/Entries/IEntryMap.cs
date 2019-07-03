﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.IO
{
    public interface IEntryMap<T> : IEntryPack<T>, IDictionary<string, T> where T : IEntryInfo
    {
        new T this[string key] { get; }

        new IEnumerable<string> Keys { get; }
        new IEnumerable<T> Values { get; }

        new bool ContainsKey(string key);
        new bool TryGetValue(string key, out T value);
    }
}
