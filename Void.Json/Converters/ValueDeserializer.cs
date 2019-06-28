using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Void.Json.Converters
{
    public abstract class ValueDeserializer<T> : ValueConverter<T>
    {
        public override bool CanWrite => false;

        public override JToken Serialize(T value, JsonSerializer serializer) {
            throw new NotSupportedException();
        }
    }
}
