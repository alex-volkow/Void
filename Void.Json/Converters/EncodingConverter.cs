using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Void.Json.Converters
{
    public class EncodingConverter : ValueConverter<Encoding>
    {
        public override Encoding Deserialize(JToken json, Type type, JsonSerializer serializer) {
            return json.Type == JTokenType.Integer
                ? Encoding.GetEncoding((int)json)
                : Encoding.GetEncoding((string)json);
        }

        public override JToken Serialize(Encoding value, JsonSerializer serializer) {
            return JToken.FromObject(value.BodyName);
        }
    }
}
