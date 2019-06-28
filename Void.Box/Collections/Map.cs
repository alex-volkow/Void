using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Collections
{
    public class Map<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IDictionary<TKey, TValue> where TValue : class
    {
        private readonly IDictionary<TKey, TValue> items;



        public virtual bool IsReadOnly => false;
        public virtual int Count => this.items.Count;
        public ICollection<TKey> Keys => this.items.Keys;
        public ICollection<TValue> Values => this.items.Values;
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => this.items.Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => this.items.Values;



        public virtual TValue this[TKey key] {
            set {
                if (ContainsKey(key)) {
                    if (value != null) {
                        this.items[key] = value;
                    }
                    else {
                        Remove(key);
                    }
                }
                else if (value != null) {
                    this.items[key] = value;
                }

            }
            get {
                return ContainsKey(key)
                    ? this.items[key]
                    : null;
            }
        }



        public Map()
            : this(null, null) {
        }

        public Map(IEnumerable<KeyValuePair<TKey, TValue>> items)
            : this(items, null) {
        }

        public Map(Func<TKey, TValue> initializer)
            : this(null, initializer) {
        }

        public Map(IEnumerable<KeyValuePair<TKey, TValue>> items, Func<TKey, TValue> initializer) {
            this.items = new Dictionary<TKey, TValue>();
            if (items != null) {
                foreach (var item in items) {
                    this[item.Key] = item.Value;
                }
            }
        }



        public virtual bool ContainsKey(TKey key) => this.items.ContainsKey(key);
        public virtual void Add(TKey key, TValue value) => this.items.Add(key, value);
        public virtual bool Remove(TKey key) => this.items.Remove(key);
        public virtual bool TryGetValue(TKey key, out TValue value) => this.items.TryGetValue(key, out value);
        public virtual void Add(KeyValuePair<TKey, TValue> item) => this.items.Add(item);
        public virtual void Clear() => this.items.Clear();
        public virtual bool Contains(KeyValuePair<TKey, TValue> item) => this.items.Contains(item);
        public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => this.items.CopyTo(array, arrayIndex);
        public virtual bool Remove(KeyValuePair<TKey, TValue> item) => this.items.Remove(item);
        public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => this.items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
