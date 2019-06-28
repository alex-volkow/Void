using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Void.Json.Converters
{
    public class ByteSizeConverter : ValueConverter<ByteSize>
    {
        public override ByteSize Deserialize(JToken json, Type type, JsonSerializer serializer) {
            return ByteSize.Parse((string)json);
        }

        public override JToken Serialize(ByteSize value, JsonSerializer serializer) {
            return JToken.FromObject(value.ToString());
        }
    }
}
