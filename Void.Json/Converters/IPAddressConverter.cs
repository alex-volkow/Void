using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Void.Json.Converters
{
    public class IPAddressConverter : ValueConverter<IPAddress>
    {
        public override IPAddress Deserialize(JToken json, Type type, JsonSerializer serializer) {
            return json != null
                ? IPAddress.Parse((string)json)
                : default(IPAddress);
        }

        public override JToken Serialize(IPAddress value, JsonSerializer serializer) {
            return JToken.FromObject(value?.ToString());
        }
    }
}
