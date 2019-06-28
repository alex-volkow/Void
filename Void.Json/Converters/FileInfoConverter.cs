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
    public class FileInfoConverter : ValueConverter<FileInfo>
    {
        public override FileInfo Deserialize(JToken json, Type type, JsonSerializer serializer) {
            return new FileInfo((string)json);
        }

        public override JToken Serialize(FileInfo value, JsonSerializer serializer) {
            return JToken.FromObject(value.FullName);
        }
    }
}
