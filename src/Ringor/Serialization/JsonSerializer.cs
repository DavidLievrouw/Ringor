using System;
using System.IO;
using System.Text;
using Dalion.Ringor.Utils;
using Newtonsoft.Json;

namespace Dalion.Ringor.Serialization {
    internal class JsonSerializer : IJsonSerializer {
        private readonly Newtonsoft.Json.JsonSerializer _inner;

        public JsonSerializer(Newtonsoft.Json.JsonSerializer inner) {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public string Serialize(object model) {
            return _inner.Serialize(model);
        }

        public string Serialize(object model, bool withIndentation) {
            return _inner.Serialize(model, withIndentation);
        }

        public T Deserialize<T>(string json) {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json))) {
                using (var reader = new StreamReader(stream)) {
                    using (var jsonReader = new JsonTextReader(reader)) {
                        return _inner.Deserialize<T>(jsonReader);
                    }
                }
            }
        }
    }
}
