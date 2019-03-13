using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dalion.Ringor.Serialization {
    /// <summary>
    ///     Converts an <see cref="Enum" /> to and from its name string value in a safe manner: if it fails anywhere in the
    ///     process, it will read or write null.
    /// </summary>
    public class StringNullableEnumArrayConverter : JsonConverter {
        private readonly StringNullableEnumConverter _stringNullableEnumConverter;

        public StringNullableEnumArrayConverter() {
            _stringNullableEnumConverter = new StringNullableEnumConverter();
        }

        public override bool CanRead => true;
        public override bool CanWrite => true;

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer) {
            if (value == null) {
                writer.WriteNull();
                return;
            }
            if (!(value is Array array)) throw new ArgumentException("Value is not an array");

            writer.WriteStartArray();
            foreach (var element in array) {
                _stringNullableEnumConverter.WriteJson(writer, element, serializer);
            }
            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer) {
            if (reader.TokenType == JsonToken.Null) {
                return null;
            }

            var values = new List<object>();
            var elementType = objectType.GetElementType();
            try {
                if (reader.TokenType == JsonToken.StartArray) {
                    while (reader.Read() && reader.TokenType != JsonToken.EndArray) {
                        var nextEnumValue = _stringNullableEnumConverter.ReadJson(reader, elementType, null, serializer);
                        values.Add(nextEnumValue);
                    }

                    var array = Array.CreateInstance(elementType, values.Count);
                    for (var i = 0; i < values.Count; i++) {
                        array.SetValue(values[i], i);
                    }
                    return array;
                }

                // didn't pass in an array. Empty array it is.
                return values.ToArray();
            }
            catch (Exception) {
                // something went horribly wrong during parsing. Empty array it is.
                return values.ToArray();
            }
        }

        public override bool CanConvert(Type type) {
            return type.IsArray && type.HasElementType && _stringNullableEnumConverter.CanConvert(type.GetElementType());
        }
    }
}
