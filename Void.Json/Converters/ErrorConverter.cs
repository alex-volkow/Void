using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Void.Reflection;

namespace Void.Json.Converters
{
    public class ErrorConverter : ValueConverter<IError>
    {
        public override IError Deserialize(JToken json, Type type, JsonSerializer serializer) {
            var proxy = json.ToObject<Proxy>(serializer);
            var errorType = Type.GetType(proxy.Type) ?? throw new JsonException(
                $"Type '{proxy.Type}' is no found"
                );
            return new ErrorInfo(errorType, proxy.Message, proxy.Content);
        }

        public override JToken Serialize(IError value, JsonSerializer serializer) {
            var proxy = new Proxy { 
                Type = value.Type.GetNameWithAssemblies(),
                Message = value.Message,
                Content = value.Content
            };
            return JToken.FromObject(proxy, serializer);
        }

        private class Proxy 
        {
            [JsonProperty(Required = Required.Always)]
            public string Type { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Message { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Content { get; set; }
        }
    }
}
