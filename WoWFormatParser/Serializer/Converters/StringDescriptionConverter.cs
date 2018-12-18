using System;
using Newtonsoft.Json;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Serializer.Converters
{
    internal class StringDescriptionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IStringDescriptor).IsAssignableFrom(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (value != null)
                writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
