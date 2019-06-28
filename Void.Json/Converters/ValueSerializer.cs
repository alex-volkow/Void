using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Void.Json.Converters
{
    public abstract class ValueSerializer<T> : ValueConverter<T>
    {
        public override bool CanRead => false;

        public override T Deserialize(JToken json, Type type, JsonSerializer serializer) {
            throw new NotSupportedException();
        }
    }
}
