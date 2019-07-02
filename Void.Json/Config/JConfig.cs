using Void.IO;
using Void.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Json
{
    /// <summary>
    /// Provides access to JToken as a configuration with dynamic fields and behaviour.
    /// </summary>
    public sealed class JConfig
    {
        private readonly JOption root;

        /// <summary>
        /// Get count of options (not recursive).
        /// </summary>
        public int Count => this.root.Count;

        /// <summary>
        /// Get a option by name.
        /// </summary>
        /// <param name="option">Option name.</param>
        /// <returns>Real or dynamic option (never null).</returns>
        public JOption this[string option] => this.root[option];

        /// <summary>
        /// Get a option by index.
        /// </summary>
        /// <param name="index">Option index.</param>
        /// <returns>Real or dynamic option (never null).</returns>
        public JOption this[int index] => this.root[index];




        internal JConfig()
            : this(new JObject()) {
        }

        /// <summary>
        /// Create a new instance with JToken
        /// </summary>
        public JConfig(JToken source) 
            : this(new JOption(source)) {
        }

        private JConfig(JOption option) {
            this.root = option;
        }


        /// <summary>
        /// Convert to string without formatting.
        /// </summary>
        public override string ToString() {
            return ToString(Formatting.None);
        }

        /// <summary>
        /// Convert to string with or without pretty formatting.
        /// </summary>
        /// <param name="pretty">Use or not pretty formatting.</param>
        public string ToString(bool pretty) {
            return ToString(pretty ? Formatting.Indented : Formatting.None);
        }

        /// <summary>
        /// Convert to string using Newtonsoft.Json.Formatting.
        /// </summary>
        /// <param name="formatting">Required formatting.</param>
        public string ToString(Formatting formatting) {
            return this.root.ToString(formatting) ?? string.Empty;
        }
    }
}
