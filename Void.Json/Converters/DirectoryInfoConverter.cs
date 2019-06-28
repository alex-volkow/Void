using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Void.Json.Converters
{
    public class DirectoryInfoConverter : ValueConverter<DirectoryInfo>
    {
        public override DirectoryInfo Deserialize(JToken json, Type type, JsonSerializer serializer) {
            return new DirectoryInfo((string)json);
        }

        public override JToken Serialize(DirectoryInfo value, JsonSerializer serializer) {
            return JToken.FromObject(value.FullName);
        }
    }
}
