using System;
using Dalion.Ringor.Utils;
using Newtonsoft.Json;

namespace Dalion.Ringor.Api.Serialization {
    public static class PreConfiguredJsonSerializer {
        public static IJsonSerializer Create() {
            return Create(_ => { });
        }

        public static IJsonSerializer Create(Action<JsonSerializerSettings> modifySettings) {
            var jsonSerializerSettings = PreConfiguredJsonSerializerSettings.Create();
            modifySettings(jsonSerializerSettings);
            return new JsonSerializer(Newtonsoft.Json.JsonSerializer.Create(jsonSerializerSettings));
        }
    }
}
