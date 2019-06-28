using Void.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Json
{
    public partial class JOption : 
        //IList<object>, 
        IReadOnlyList<JOption>, 
        //IDictionary<string, object>, 
        IReadOnlyDictionary<string, JOption>,
        IEquatable<JOption>, 
        IEquatable<JToken>,
        ICloneable
    {
        public string Path => this.path.Value;

        //public IList<object> List => this;

        //public IDictionary<string, object> Map => this;

        public bool IsReadOnly => true;

        public bool IsExist {
            get {
                lock (this.locker) {
                    return GetElement() != null;
                }
            }
        }

        public bool IsNull {
            get {
                lock (this.locker) {
                    var element = GetElement();
                    return element == null || element.Type == JTokenType.Null;
                }
            }
        }

        public int Count {
            get {
                lock (this.locker) {
                    return ToJson() is JContainer container
                        ? container.Count
                        : default(int);
                }
            }
        }

        public dynamic Value {
            get {
                lock (this.locker) {
                    return (dynamic)this.GetElement();
                }
            }
        }


        public JOption this[string option] {
            get {
                lock (this.locker) {
                    return new JElement(this, option);
                }
            }
        }

        public JOption this[int index] {
            get {
                lock (this.locker) {
                    return new JIndex(this, index);
                }
            }
        }





        public virtual object Clone() {
            lock (this.locker) {
                return new JOption(this.Source);
            }
        }

        public virtual bool Equals(JOption other) {
            return other != null && JToken.DeepEquals(GetElement(), other?.GetElement());
        }

        public bool Equals(JToken other) {
            return other != null && JToken.DeepEquals(GetElement(), other);
        }

        public override bool Equals(object obj) {
            if (obj is JOption option) {
                return Equals(option);
            }
            if (obj is JToken json) {
                return Equals(json);
            }
            return object.Equals(obj, To<object>());
        }

        public override int GetHashCode() {
            return GetElement()?.GetHashCode() ?? 0;
        }

        public override string ToString() {
            return ToString(Formatting.None);
        }

        public string ToString(bool pretty) {
            return ToString(pretty ? Formatting.Indented : Formatting.None);
        }


        public string ToString(Formatting formatting) {
            lock (this.locker) {
                return GetElement()?.ToString(formatting) ?? string.Empty;
            }
        }

        public JToken ToJson() {
            lock (this.locker) {
                return GetElement()?.DeepClone();
            }
        }

        public object To(Type type) {
            lock (this.locker) {
                var element = GetElement();
                return element != null && element.Type != JTokenType.Null
                    ? element.ToObject(type)
                    : type.GetDefaultValue();
            }
        }

        public T To<T>() {
            return (T)To(typeof(T));
        }

        public T Required<T>() {
            lock (this.locker) {
                return Required().To<T>();
            }
        }

        public JOption Required() {
            lock (this.locker) {
                if (this.IsNull) {
                    throw new ArgumentNullException(this.Path);
                }
                return this;
            }
        }

        public IEnumerator<JOption> GetEnumerator() {
            return GetItems().GetEnumerator();
        }
    }
}
