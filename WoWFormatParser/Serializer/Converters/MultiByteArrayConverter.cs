using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WoWFormatParser.Serializer.Converters
{
    internal class MultiArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int[,]) ||
                   objectType == typeof(float[,]) ||
                   objectType == typeof(uint[,]) ||
                   objectType == typeof(byte[,]) ||
                   objectType == typeof(string[,]);
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value is Array data)
            {
                writer.WriteStartArray();

                if (data.Length > 0)
                {
                    int rowlen = data.GetLength(0);
                    for (int i = 0; i < rowlen; i++)
                        writer.WriteValue("[" + string.Join(", ", GetValues(data, i)) + "]");
                }

                writer.WriteEndArray();
            }
        }

        private IEnumerable<object> GetValues(Array data, int row)
        {
            int cols = data.GetLength(1);
            for (int i = 0; i < cols; i++)
                yield return data.GetValue(row, i);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
