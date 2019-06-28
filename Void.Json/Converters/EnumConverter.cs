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
    public class EnumConverter : JsonConverter
    {
        public override bool CanConvert(Type type) {
            return type.IsEnum || (type.IsNullable() && (Nullable.GetUnderlyingType(type)?.IsEnum ?? false));
        }

        public override object ReadJson(JsonReader reader, Type type, object value, JsonSerializer serializer) {
            var json = JToken.ReadFrom(reader);
            if (json == null || json.Type == JTokenType.Null) {
                if (type.IsEnum) {
                    throw new JsonSerializationException(
                        $"Failed to serialize NULL value to {type.Name}"
                        );
                }
                return null;
            }
            if (type.IsNullable()) {
                type = Nullable.GetUnderlyingType(type);
            }
            return Enum.Parse(type, (string)json, true);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            if (value != null) {
                var json = JToken.FromObject(value.ToString());
                json.WriteTo(writer);
            }
        }
    }
}
