using Newtonsoft.Json;
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
        private static readonly char[] SpecialCharacters = {
            '.', ' ', '\'', '/', '"', '[', ']', '(', ')',
            '\t', '\n', '\r', '\f', '\b', '\\',
            '\u0085', '\u2028', '\u2029'
        };

        private static readonly char[] Escapters = { '\\', '\'' };


        protected internal readonly object locker;
        private readonly Lazy<string> path;
        private readonly JOption parent;
        private JToken source;


        protected JToken Source {
            get {
                var option = this;
                while (option.parent != null) {
                    option = option.parent;
                }
                return option.source;
            }
        }



        protected internal JOption(JOption parent) {
            this.locker = parent?.locker ?? new object();
            this.parent = parent;
            this.path = new Lazy<string>(GetPath);
        }

        internal JOption(JToken source)
            : this(default(JOption)) {
            this.source = source?.DeepClone();
        }



        private IReadOnlyList<JOption> GetItems() {
            lock (this.locker) {
                if (ToJson() is JContainer container && container != null) {
                    var items = new List<JOption>(container.Count);
                    if (container is JArray array) {
                        for (var i = 0; i < container.Count; i++) {
                            items.Add(new JIndex(this, i));
                        }
                    }
                    else if (container is JObject element) {
                        foreach (var item in element) {
                            items.Add(new JElement(this, item.Key));
                        }
                    }
                    return items;
                }
                else {
                    return new JOption[] { };
                }
            }
        }

        //private IList<JToken> AsList() {
        //    lock (this.locker) {
        //        return GetElement() is JArray array ? array : null;
        //    }
        //}

        //private IDictionary<string, JToken> AsMap() {
        //    lock (this.locker) {
        //        return GetElement() is JObject map ? map : null;
        //    }
        //}

        //private JToken Serialize(object value) {
        //    return value != null
        //        ? JToken.FromObject(value)
        //        : new JValue(default(object));
        //}

        //private JArray CreateArray(int index, object value) {
        //    var array = new object[index + 1];
        //    array[index] = Serialize(value);
        //    return new JArray(array); 
        //}

        

        private JToken GetElement() {
            lock (this.locker) {
                var json = this.Source;
                var nodes = GetLineage();
                for (var i = 0; i < nodes.Count && json != null && json.Type != JTokenType.Null; i++) {
                    if (nodes[i] is JElement element) {
                        if (json is JObject container && container.ContainsKey(element.Name)) {
                            json = container[element.Name];
                            continue;
                        }
                        return null;
                    }
                    if (nodes[i] is JIndex index) {
                        if (json is JArray array && index.Index >= 0 && index.Index < array.Count) {
                            json = array[index.Index]/* ?? new JValue(default(object))*/;
                        }
                        else {
                            return null;
                        }
                        continue;
                    }
                }
                return json;
            }
        }

        private string GetPath() {
            lock (this.locker) {
                var path = new StringBuilder();
                foreach (var node in GetLineage()) {
                    if (node is JElement element) {
                        if (path.Length > 0) {
                            path.Append('.');
                        }
                        if (element.Name.IndexOfAny(SpecialCharacters) != -1) {
                            path.Append("['");
                            if (element.Name.IndexOfAny(Escapters) != -1) {
                                path.Append(element.Name
                                    .Replace(@"\", @"\\")
                                    .Replace(@"'", @"\'")
                                    );
                            }
                            else {
                                path.Append(element.Name);
                            }
                            path.Append("']");
                        }
                        else {
                            path.Append(element.Name);
                        }
                        continue;
                    }
                    if (node is JIndex index) {
                        path.Append('[');
                        path.Append(index.Index);
                        path.Append(']');
                        continue;
                    }
                }
                return path.ToString();
            }
        }

        private IReadOnlyList<JOption> GetLineage() {
            lock (this.locker) {
                var nodes = new List<JOption>();
                var node = this;
                while (node != null) {
                    nodes.Add(node);
                    node = node.parent;
                }
                nodes.Reverse();
                return nodes;
            }
        }
    }
}
