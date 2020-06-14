using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EveryBus.Domain.Converters
{
    public class StopIdToIntConverter : JsonConverter<int?>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(int?) == typeToConvert;
        }
        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();

                if (Int32.TryParse(value, out var number))
                {
                    return number;
                }

                return null;
            }

            return reader.GetInt32();
        }

        public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}