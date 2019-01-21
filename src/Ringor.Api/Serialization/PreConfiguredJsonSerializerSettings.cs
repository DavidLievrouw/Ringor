using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Dalion.Ringor.Api.Serialization {
    public static class PreConfiguredJsonSerializerSettings {
        public static JsonSerializerSettings Create() {
            var settings = new JsonSerializerSettings();
            Apply(settings);
            return settings;
        }

        public static void Apply(JsonSerializerSettings settings) {
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Formatting = Formatting.Indented;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.Converters.Add(new StringNullableEnumConverter());
            settings.Converters.Add(new StringNullableEnumArrayConverter());
            settings.Converters.Add(new StringEnumConverter());
            foreach (var customJsonConverter in CustomConverters()) {
                settings.Converters.Add(customJsonConverter);
            }
        }

        private static IEnumerable<JsonConverter> CustomConverters() {
            return new JsonConverter[] {
                // Register custom json serializers here
            };
        }
    }
}