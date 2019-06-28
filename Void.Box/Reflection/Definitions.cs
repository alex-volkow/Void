using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Void.Reflection
{
    public class Definitions : IDefinitions, IDictionary<Type, object>
    {
        private readonly Dictionary<Type, object> items;
        private readonly object locker;



        public int Count {
            get {
                lock (this.locker) {
                    return this.items.Count;
                }
            }
        }

        public IReadOnlyCollection<Type> Types => GetTypes();

        bool ICollection<KeyValuePair<Type, object>>.IsReadOnly => false;
        ICollection<Type> IDictionary<Type, object>.Keys => GetTypes();
        ICollection<object> IDictionary<Type, object>.Values => GetValues();
        int ICollection<KeyValuePair<Type, object>>.Count => this.Count;
        IEnumerable<Type> IReadOnlyDictionary<Type, object>.Keys => GetTypes();
        IEnumerable<object> IReadOnlyDictionary<Type, object>.Values => GetValues();
        int IReadOnlyCollection<KeyValuePair<Type, object>>.Count => this.Count;

        object IReadOnlyDictionary<Type, object>.this[Type key] => Get(key);

        object IDictionary<Type, object>.this[Type key] {
            get => Get(key);
            set => Set(key, value);
        }



        public Definitions()
            : this(null) {
        }

        public Definitions(Definitions definitions) {
            this.items = definitions?.items ?? new Dictionary<Type, object>();
            this.locker = definitions?.locker ?? new object();
        }



        public static Definitions Copy(IDefinitions services) {
            var clone = new Definitions();
            foreach (var service in services) {
                clone.Set(service.Key, service.Value);
            }
            return clone;
        }

        public object Create(Type type) {
            return Create(type, null);
        }

        public object Create(Type type, params object[] args) {
            if (type == null) {
                throw new ArgumentNullException(nameof(type));
            }
            var bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            return Activator.CreateInstance(type, bindings, null, args, null);
        }

        public T Create<T>() {
            return (T)Create(typeof(T));
        }

        public T Create<T>(params object[] args) {
            return (T)Create(typeof(T), args);
        }

        public void Replace(IDefinitions definitions) {
            if (definitions != null) {
                foreach (var definition in definitions) {
                    Set(definition.Key, definition.Value);
                }
            }
        }

        public bool Remove(Type type) {
            lock (this.locker) {
                return this.items.Remove(type);
            }
        }

        public void Set<TDefinition, TImplementation>()
            where TDefinition : class
            where TImplementation : TDefinition, new() {
            Set<TDefinition, TImplementation>(new TImplementation());
        }

        public void Set<TDefinition, TImplementation>(TImplementation implementation)
            where TDefinition : class
            where TImplementation : TDefinition {
            Set<TDefinition>(implementation);
        }

        public void Set<T>(T implementation) where T : class {
            Set(typeof(T), implementation);
        }

        public void Set(Type type, object implementation) {
            if (type == null) {
                throw new ArgumentNullException(nameof(type));
            }
            if (implementation == null) {
                throw new ArgumentNullException(nameof(implementation));
            }
            else {
                lock (this.locker) {
                    if (type.IsValueType) {
                        throw new InvalidOperationException(
                            $"Type must be a class"
                            );
                    }
                    if (this.items.ContainsKey(type)) {
                        throw new InvalidOperationException(
                            $"Type '{type}' already registered"
                            );
                    }
                    this.items.Add(type, implementation);
                }
            }
        }

        public bool Contains(Type type) {
            lock (this.locker) {
                return this.items.ContainsKey(type);
            }
        }

        public object Get(Type type) {
            lock (this.locker) {
                return this.items.ContainsKey(type)
                    ? this.items[type]
                    : default(object);
            }
        }

        public T Get<T>() {
            return (T)Get(typeof(T));
        }

        public object GetRequired(Type type) {
            return Get(type) ?? throw new ArgumentException(
                $"Implementation required: {type.FullName}"
                );
        }

        public T GetRequired<T>() {
            return (T)GetRequired(typeof(T));
        }

        public void Clear() {
            lock (this.locker) {
                this.items.Clear();
            }
        }

        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator() {
            lock (this.locker) {
                return this.items
                    .ToArray()
                    .Cast<KeyValuePair<Type, object>>()
                    .GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        void IDictionary<Type, object>.Add(Type key, object value) => Set(key, value);
        bool IDictionary<Type, object>.ContainsKey(Type key) => Contains(key);
        void ICollection<KeyValuePair<Type, object>>.Add(KeyValuePair<Type, object> item) => Set(item.Key, item.Value);
        void ICollection<KeyValuePair<Type, object>>.Clear() => Clear();
        bool ICollection<KeyValuePair<Type, object>>.Contains(KeyValuePair<Type, object> item) => Contains(item.Key);
        bool IReadOnlyDictionary<Type, object>.ContainsKey(Type key) => Contains(key);
        bool ICollection<KeyValuePair<Type, object>>.Remove(KeyValuePair<Type, object> item) => Remove(item.Key);

        public void CopyTo(KeyValuePair<Type, object>[] array, int arrayIndex) {
            lock (this.locker) {
                var items = (ICollection<KeyValuePair<Type, object>>)this.items;
                items.CopyTo(array, arrayIndex);
            }
        }

        public bool TryGetValue(Type key, out object value) {
            lock (this.locker) {
                return this.items.TryGetValue(key, out value);
            }
        }

        private Type[] GetTypes() {
            lock (this.locker) {
                return this.items.Keys.ToArray();
            }
        }

        private object[] GetValues() {
            lock (this.locker) {
                return this.items.Values.ToArray();
            }
        }
    }
}
