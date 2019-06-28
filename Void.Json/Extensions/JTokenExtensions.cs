using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Void.Json
{
    public static class JTokenExtensions
    {
        public static T? GetValue<T>(this JToken json, string name) where T : struct {
            return json.GetValue<T>(name, null);
        }

        public static T? GetValue<T>(this JToken json, string name, JsonSerializer serializer) where T : struct {
            return serializer != null
                ? json[name]?.ToObject<T>(serializer)
                : json[name]?.ToObject<T>();
        }

        public static T GetObject<T>(this JToken json, string name) where T : class {
            return json.GetObject<T>(name, null);
        }

        public static T GetObject<T>(this JToken json, string name, JsonSerializer serializer) where T : class {
            return serializer != null
                ? json[name]?.ToObject<T>(serializer)
                : json[name]?.ToObject<T>();
        }

        public static T GetRequired<T>(this JToken json, string name) {
            return json.GetRequired<T>(name, null);
        }

        public static T GetRequired<T>(this JToken json, string name, JsonSerializer serializer) {
            var item = json[name] ?? throw new JsonException($"JSON property required: {name}");
            return serializer != null
                ? item.ToObject<T>(serializer)
                : item.ToObject<T>();
        }

        public static JObject Set<T>(this JObject json, Expression<Func<T>> member) {
            return json.Set(member, null);
        }

        public static JObject Set<T>(this JObject json, Expression<Func<T>> member, JsonSerializer serializer) {
            var body = (MemberExpression)member.Body;
            var value = member.Compile();
            return json.Set(body.Member.Name, value(), serializer);
        }

        public static JObject Set(this JObject json, string name, object value, JsonSerializer serializer) {
            if (json != null && name != null) {
                if (value != null || serializer?.NullValueHandling == NullValueHandling.Include) {
                    if (value != null) {
                        json[name] = serializer != null
                            ? JToken.FromObject(value, serializer)
                            : JToken.FromObject(value);
                    }
                    else {
                        json[name] = null;
                    }
                }
            }
            return json;
        }

        public static JObject SetRequired<T>(this JObject json, Expression<Func<T>> member) {
            return json.SetRequired(member, null);
        }

        public static JObject SetRequired<T>(this JObject json, Expression<Func<T>> member, JsonSerializer serializer) {
            var body = (MemberExpression)member.Body;
            var value = member.Compile();
            return json.SetRequired(body.Member.Name, value(), serializer);
        }

        public static JObject SetRequired(this JObject json, string name, object value) {
            return json.SetRequired(name, value, null);
        }

        public static JObject SetRequired(this JObject json, string name, object value, JsonSerializer serializer) {
            if (json == null) {
                throw new ArgumentNullException(nameof(json));
            }
            if (name == null) {
                throw new ArgumentNullException(nameof(name));
            }
            if (value == null) {
                throw new JsonException(
                    $"Property '{name}' must have value"
                    );
            }
            json[name] = serializer != null
                ? JToken.FromObject(value, serializer)
                : JToken.FromObject(value);
            return json;
        }
    }
}
