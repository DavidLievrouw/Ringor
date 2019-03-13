using System.IO;
using Newtonsoft.Json;

namespace Dalion.Ringor.Serialization {
    internal static partial class Extensions {
        public static string Serialize(this Newtonsoft.Json.JsonSerializer jsonSerializer, object model) {
            return jsonSerializer.Serialize(model, false);
        }

        public static string Serialize(this Newtonsoft.Json.JsonSerializer jsonSerializer, object model, bool withIndentation) {
            using (var outputStream = new MemoryStream()) {
                jsonSerializer.Serialize(model, outputStream, withIndentation);
                outputStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(outputStream)) {
                    return reader.ReadToEnd();
                }
            }
        }

        private static void Serialize(this Newtonsoft.Json.JsonSerializer jsonSerializer, object model, Stream outputStream, bool withIndentation) {
            using (var writer = new StreamWriter(new UnclosableStreamWrapper(outputStream))) {
                using (var jsonWriter = new JsonTextWriter(writer)) {
                    jsonWriter.Formatting = withIndentation
                        ? Formatting.Indented
                        : Formatting.None;
                    jsonSerializer.Formatting = withIndentation
                        ? Formatting.Indented
                        : Formatting.None;
                    jsonSerializer.Serialize(jsonWriter, model);
                }
            }
        }
    }
}
