using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Void.Json
{
    public static class SettingsExtensions
    {
        public static JsonSerializerSettings GetSettings(this JsonSerializer source) {
            var settings = new JsonSerializerSettings {
                Context = source.Context,
                Culture = source.Culture,
                ContractResolver = source.ContractResolver,
                ConstructorHandling = source.ConstructorHandling,
                CheckAdditionalContent = source.CheckAdditionalContent,
                DateFormatHandling = source.DateFormatHandling,
                DateFormatString = source.DateFormatString,
                DateParseHandling = source.DateParseHandling,
                DateTimeZoneHandling = source.DateTimeZoneHandling,
                DefaultValueHandling = source.DefaultValueHandling,
                EqualityComparer = source.EqualityComparer,
                FloatFormatHandling = source.FloatFormatHandling,
                Formatting = source.Formatting,
                FloatParseHandling = source.FloatParseHandling,
                MaxDepth = source.MaxDepth,
                MetadataPropertyHandling = source.MetadataPropertyHandling,
                MissingMemberHandling = source.MissingMemberHandling,
                NullValueHandling = source.NullValueHandling,
                ObjectCreationHandling = source.ObjectCreationHandling,
                PreserveReferencesHandling = source.PreserveReferencesHandling,
                //ReferenceResolver = serializer.ReferenceResolver,
                ReferenceLoopHandling = source.ReferenceLoopHandling,
                StringEscapeHandling = source.StringEscapeHandling,
                TraceWriter = source.TraceWriter,
                TypeNameHandling = source.TypeNameHandling,
                SerializationBinder = source.SerializationBinder,
                TypeNameAssemblyFormatHandling = source.TypeNameAssemblyFormatHandling
            };
            foreach (var converter in source.Converters) {
                settings.Converters.Add(converter);
            }
            return settings;
        }

        public static JsonSerializer Clone(this JsonSerializer source) {
            return JsonSerializer.Create(source.GetSettings());
        }

        public static JsonSerializerSettings Clone(this JsonSerializerSettings source) {
            var settings = new JsonSerializerSettings {
                Context = source.Context,
                Culture = source.Culture,
                ContractResolver = source.ContractResolver,
                ConstructorHandling = source.ConstructorHandling,
                CheckAdditionalContent = source.CheckAdditionalContent,
                DateFormatHandling = source.DateFormatHandling,
                DateFormatString = source.DateFormatString,
                DateParseHandling = source.DateParseHandling,
                DateTimeZoneHandling = source.DateTimeZoneHandling,
                DefaultValueHandling = source.DefaultValueHandling,
                EqualityComparer = source.EqualityComparer,
                FloatFormatHandling = source.FloatFormatHandling,
                Formatting = source.Formatting,
                FloatParseHandling = source.FloatParseHandling,
                MaxDepth = source.MaxDepth,
                MetadataPropertyHandling = source.MetadataPropertyHandling,
                MissingMemberHandling = source.MissingMemberHandling,
                NullValueHandling = source.NullValueHandling,
                ObjectCreationHandling = source.ObjectCreationHandling,
                PreserveReferencesHandling = source.PreserveReferencesHandling,
                //ReferenceResolver = serializer.ReferenceResolver,
                ReferenceLoopHandling = source.ReferenceLoopHandling,
                StringEscapeHandling = source.StringEscapeHandling,
                TraceWriter = source.TraceWriter,
                TypeNameHandling = source.TypeNameHandling,
                SerializationBinder = source.SerializationBinder,
                TypeNameAssemblyFormatHandling = source.TypeNameAssemblyFormatHandling
            };
            foreach (var converter in source.Converters) {
                settings.Converters.Add(converter);
            }
            return settings;
        }
    }
}
