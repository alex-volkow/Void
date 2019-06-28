using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Json
{
    public partial class JOption
    {

        //ICollection<string> IDictionary<string, object>.Keys => throw new NotImplementedException();

        //ICollection<object> IDictionary<string, object>.Values => throw new NotImplementedException();

        IEnumerable<string> IReadOnlyDictionary<string, JOption>.Keys {
            get {
                lock (this.locker) {
                    return GetElement() is IDictionary<string, JToken> map
                        ? map.Keys
                        : Enumerable.Empty<string>();
                }
            }
        }

        IEnumerable<JOption> IReadOnlyDictionary<string, JOption>.Values {
            get {
                lock (this.locker) {
                    return GetElement() is IDictionary<string, JToken>
                        ? GetItems()
                        : Enumerable.Empty<JOption>();
                }
            }
        }

        int IReadOnlyCollection<KeyValuePair<string, JOption>>.Count {
            get {
                lock (this.locker) {
                    return GetElement() is IDictionary<string, JToken> map
                        ? map.Count
                        : 0;
                }
            }
        }

        JOption IReadOnlyDictionary<string, JOption>.this[string key] => this[key];

        //object IDictionary<string, object>.this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        bool IReadOnlyDictionary<string, JOption>.ContainsKey(string key) {
            lock (this.locker) {
                return GetElement() is IDictionary<string, JToken> map
                    ? map.ContainsKey(key)
                    : false;
            }
        }

        bool IReadOnlyDictionary<string, JOption>.TryGetValue(string key, out JOption value) {
            lock (this.locker) {
                value = GetElement() is IDictionary<string, JToken> ? this[key] : new JElement(this, key);
                return !value.IsNull;
                //if (GetElement() is IDictionary<string, JToken>) {
                //    value = this[key];
                //    return !value.IsNull;
                //}
                //return false;
            }
        }

        //bool IDictionary<string, object>.ContainsKey(string key) {
        //    throw new NotImplementedException();
        //}

        //void IDictionary<string, object>.Add(string key, object value) {
        //    throw new NotImplementedException();
        //}

        //bool IDictionary<string, object>.Remove(string key) {
        //    throw new NotImplementedException();
        //}

        //bool IDictionary<string, object>.TryGetValue(string key, out object value) {
        //    throw new NotImplementedException();
        //}

        //void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item) {
        //    throw new NotImplementedException();
        //}

        //void ICollection<KeyValuePair<string, object>>.Clear() {
        //    throw new NotImplementedException();
        //}

        //bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item) {
        //    throw new NotImplementedException();
        //}

        //void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) {
        //    throw new NotImplementedException();
        //}

        //bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item) {
        //    throw new NotImplementedException();
        //}

        IEnumerator<KeyValuePair<string, JOption>> IEnumerable<KeyValuePair<string, JOption>>.GetEnumerator() {
            lock (this.locker) {
                var items = new List<KeyValuePair<string, JOption>>();
                if (GetElement() is IDictionary<string, JToken> map) {
                    foreach (var item in map) {
                        items.Add(new KeyValuePair<string, JOption>(
                            item.Key, new JElement(this, item.Key)
                            ));
                    }
                }
                return items.GetEnumerator();
            }
        }
    }
}
