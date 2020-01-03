using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Void.IO;

namespace Void.Json.Converters
{
    public class FilePathConverter : ValueConverter<FilePath>
    {
        public override FilePath Deserialize(JToken json, Type type, JsonSerializer serializer) {
            return new FilePath((string)json);
        }

        public override JToken Serialize(FilePath value, JsonSerializer serializer) {
            return JToken.FromObject(value.ToString());
        }
    }
}
