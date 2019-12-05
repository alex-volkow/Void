using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Collections
{
    /// <summary>
    /// Dictionary without null values.
    /// </summary>
    public class Map<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IDictionary<TKey, TValue> where TValue : class
    {
        private readonly IDictionary<TKey, TValue> items;


        /// <inheritdoc />
        public virtual bool IsReadOnly => false;

        /// <inheritdoc />
        public virtual int Count => this.items.Count;

        /// <inheritdoc />
        public ICollection<TKey> Keys => this.items.Keys;

        /// <inheritdoc />
        public ICollection<TValue> Values => this.items.Values;

        /// <inheritdoc />
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => this.items.Keys;

        /// <inheritdoc />
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => this.items.Values;


        /// <summary>
        /// Gets or sets the element with the specified key. If value is null the key will be removed.
        /// </summary>
        public virtual TValue this[TKey key] {
            set {
                if (value != null) {
                    this.items[key] = value;
                }
                else {
                    Remove(key);
                }

            }
            get {
                return TryGetValue(key, out TValue value)
                    ? value
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


        /// <inheritdoc />
        public virtual bool ContainsKey(TKey key) => this.items.ContainsKey(key);

        /// <summary>
        /// Adds an element with the provided key and value if value is not null.
        /// </summary>
        public virtual void Add(TKey key, TValue value) {
            if (value != null) {
                this.items.Add(key, value);
            }
        }

        /// <inheritdoc />
        public virtual bool Remove(TKey key) => this.items.Remove(key);

        /// <inheritdoc />
        public virtual bool TryGetValue(TKey key, out TValue value) => this.items.TryGetValue(key, out value);

        /// <inheritdoc />
        public virtual void Add(KeyValuePair<TKey, TValue> item) => this.items.Add(item);

        /// <inheritdoc />
        public virtual void Clear() => this.items.Clear();

        /// <inheritdoc />
        public virtual bool Contains(KeyValuePair<TKey, TValue> item) => this.items.Contains(item);

        /// <inheritdoc />
        public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => this.items.CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public virtual bool Remove(KeyValuePair<TKey, TValue> item) => this.items.Remove(item);

        /// <inheritdoc />
        public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => this.items.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
