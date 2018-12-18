using System;
using Newtonsoft.Json;

namespace WoWFormatParser.Serializer.Converters
{
    internal class ByteArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(byte[]);

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value is byte[] data)
                writer.WriteValue("[" + string.Join(", ", data) + "]");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
