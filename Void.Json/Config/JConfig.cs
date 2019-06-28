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
    public sealed class JConfig
    {
        private readonly JOption root;


        public int Count => this.root.Count;

        public JOption this[string option] => this.root[option];

        public JOption this[int index] => this.root[index];




        public JConfig()
            : this(new JObject()) {
        }

        public JConfig(JToken source) 
            : this(new JOption(source)) {
        }

        private JConfig(JOption option) {
            this.root = option;
        }



        public override string ToString() {
            return ToString(Formatting.None);
        }

        public string ToString(bool pretty) {
            return ToString(pretty ? Formatting.Indented : Formatting.None);
        }

        public string ToString(Formatting formatting) {
            return this.root.ToString(formatting) ?? string.Empty;
        }
    }
}
