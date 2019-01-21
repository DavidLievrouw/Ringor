using System;
using Newtonsoft.Json;

namespace Dalion.Ringor.Api.Serialization {
  /// <summary>
  /// Converts an <see cref="Enum"/> to and from its name string value in a safe manner: if it fails anywhere in the process, it will read or write null.
  /// </summary>
  public class StringNullableEnumConverter : JsonConverter {
    public override bool CanRead => true;
    public override bool CanWrite => true;

    public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer) {
        if (value == null) {
        writer.WriteNull();
        return;
        }

      var e = (Enum) value;
      writer.WriteValue(e.ToString());
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer) {
      if (reader.TokenType == JsonToken.Null) {
        return null;
      }

      Type type = Nullable.GetUnderlyingType(objectType);
      if (type == null) return null;

      try {
        if (reader.TokenType == JsonToken.String || reader.TokenType == JsonToken.Integer) {
          var enumText = reader.Value.ToString();

          if (enumText == string.Empty) return null;

          var enumValue = Enum.Parse(type, enumText, true);

          if (!Enum.IsDefined(type, enumValue))
            return null;

          return enumValue;
        }

        // did not pass in an integer or string, just return null
        return null;
      }
      catch (Exception) {
        // failed to parse some text or integer to an enum value? OK, continue with null.
        return null;
      }
    }

    public override bool CanConvert(Type type) => IsNullableType(type) && UnwrapNullableType(type).IsEnum;

    private static bool IsNullableType(Type type) => type.IsValueType && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

    private static Type UnwrapNullableType(Type type) => Nullable.GetUnderlyingType(type);
  }
}
