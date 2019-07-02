using Void.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Void.Json
{
    /// <summary>
    /// Represents real or dynamic item of a configuration.
    /// </summary>
    public partial class JOption : 
        //IList<object>, 
        IReadOnlyList<JOption>, 
        //IDictionary<string, object>, 
        IReadOnlyDictionary<string, JOption>,
        IEquatable<JOption>, 
        IEquatable<JToken>,
        ICloneable
    {
        /// <summary>
        /// Get full name of the option.
        /// </summary>
        public string Path => this.path.Value;

        //public IList<object> List => this;

        //public IDictionary<string, object> Map => this;

        public bool IsReadOnly => true;

        /// <summary>
        /// True if the option has any value (included null), else False.
        /// </summary>
        public bool IsExist {
            get {
                lock (this.locker) {
                    return GetElement() != null;
                }
            }
        }

        /// <summary>
        /// The option is not exist or has null value.
        /// </summary>
        public bool IsNull {
            get {
                lock (this.locker) {
                    var element = GetElement();
                    return element == null || element.Type == JTokenType.Null;
                }
            }
        }

        /// <summary>
        /// Get count of child options (not recursive).
        /// </summary>
        public int Count {
            get {
                lock (this.locker) {
                    return ToJson() is JContainer container
                        ? container.Count
                        : default(int);
                }
            }
        }

        /// <summary>
        /// Get option value as dynamic object.
        /// </summary>
        public dynamic Value {
            get {
                lock (this.locker) {
                    return (dynamic)this.GetElement();
                }
            }
        }


        /// <summary>
        /// Get a child option by name.
        /// </summary>
        /// <param name="option">Child option name.</param>
        /// <returns>Real or dynamic option (never null).</returns>
        public JOption this[string option] {
            get {
                lock (this.locker) {
                    return new JElement(this, option);
                }
            }
        }

        /// <summary>
        /// Get a child option by index.
        /// </summary>
        /// <param name="index">Child option index.</param>
        /// <returns>Real or dynamic option (never null).</returns>
        public JOption this[int index] {
            get {
                lock (this.locker) {
                    return new JIndex(this, index);
                }
            }
        }




        /// <summary>
        /// Create a deep clone.
        /// </summary>
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

        /// <summary>
        /// Get the option value as JToken.
        /// </summary>
        public JToken ToJson() {
            lock (this.locker) {
                return GetElement()?.DeepClone();
            }
        }

        /// <summary>
        /// Cast the option value to type.
        /// </summary>
        /// <returns>Instance of the type if value exists or not null else null.</returns>
        /// <exception cref="ArgumentNullException">Type is null.</exception>
        public object To(Type type) {
            if (type == null) {
                throw new ArgumentNullException(nameof(type));
            }
            lock (this.locker) {
                var element = GetElement();
                return element != null && element.Type != JTokenType.Null
                    ? element.ToObject(type)
                    : type.GetDefaultValue();
            }
        }

        /// <summary>
        /// Cast the option value to type.
        /// </summary>
        /// <returns>Instance of the type if value exists or not null else null.</returns>
        public T To<T>() {
            return (T)To(typeof(T));
        }

        /// <summary>
        /// Cast the option value to type.
        /// </summary>
        /// <returns>Instance of the type.</returns>
        /// <exception cref="InvalidDataException">The option has bull value or not exist.</exception>
        public T Required<T>() {
            lock (this.locker) {
                return Required().To<T>();
            }
        }

        /// <summary>
        /// Ensure the option is exist and has not null vlaue.
        /// </summary>
        /// <returns>Self.</returns>
        /// <exception cref="InvalidDataException">The option has bull value or not exist.</exception>
        public JOption Required() {
            lock (this.locker) {
                if (this.IsNull) {
                    throw new InvalidDataException(
                        $"Option required: {this.Path}"
                        );
                }
                return this;
            }
        }

        public IEnumerator<JOption> GetEnumerator() {
            return GetItems().GetEnumerator();
        }
    }
}
