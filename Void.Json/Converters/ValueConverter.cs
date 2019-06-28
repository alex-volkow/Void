using Void.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Json.Converters
{
    public abstract class ValueConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type type) {
            return type == typeof(T);
        }

        public override object ReadJson(JsonReader reader, Type type, object value, JsonSerializer serializer) {
            var item = JToken.ReadFrom(reader);
            return item != null || typeof(T).IsValueType || (type?.IsValueType ?? false)
                ? Deserialize(item, type, serializer)
                : default(T);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            if (value != null || typeof(T).IsValueType) {
                Serialize((T)value, serializer).WriteTo(writer);
            }
        }

        public abstract T Deserialize(JToken json, Type type, JsonSerializer serializer);
        public abstract JToken Serialize(T value, JsonSerializer serializer);
    }
}
