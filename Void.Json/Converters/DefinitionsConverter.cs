using Void.Collections;
using Void.Json.Converters;
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
    public class DefinitionsConverter : ValueConverter<IDefinitions>
    {
        public override bool CanConvert(Type type) {
            return type.Is<IDefinitions>();
        }

        public override IDefinitions Deserialize(JToken json, Type type, JsonSerializer serializer) {
            var definitions = new Definitions();
            foreach (var item in (JArray)json) {
                var key = Type.GetType(item.GetRequired<string>("Key"));
                var implementation = Type.GetType(item.GetRequired<string>("Type"));
                definitions.Set(key, item["Value"]?.ToObject(implementation));
            }
            return definitions;
        }

        public override JToken Serialize(IDefinitions value, JsonSerializer serializer) {
            var json = new JArray();
            foreach (var definition in value) {
                json.Add(new JObject {
                    ["Key"] = definition.Key.GetNameWithAssemblies(),
                    ["Type"] = definition.Value.GetType().GetNameWithAssemblies(),
                    ["Value"] = JToken.FromObject(definition.Value, serializer)
                });
            }
            return json;
            //var items = value.ToDictionary(e => e.Key.GetNameWithAssemblies(), e => e.Value);
            //if (serializer.TypeNameHandling == TypeNameHandling.None) {
            //    serializer = serializer.Clone();
            //    serializer.TypeNameHandling = TypeNameHandling.Objects;
            //}
            //return JToken.FromObject(items, serializer);
        }
        
        //public static IEnumerable<IJDefinition> Extract(JToken token) {
        //    if (token is JObject value) {
        //        yield return value.ToObject<JService>();
        //    }
        //    if (token is JArray array) {
        //        foreach (var item in array) {
        //            foreach (var service in Extract(item)) {
        //                yield return service;
        //            }
        //        }
        //    }
        //}

        //private class Service
        //{
        //    public Type Definition { get; set; }
        //    public Type Implementation { get; set; }
        //    public JToken Value { get; set; }
        //}

        //private class JService : IJDefinition
        //{
        //    [JsonProperty(Required = Required.Always)]
        //    public string Definition { get; set; }

        //    [JsonProperty(Required = Required.Always)]
        //    public string Implementation { get; set; }

        //    [JsonProperty(Required = Required.Always)]
        //    public JToken Value { get; set; }
        //}
    }
}
