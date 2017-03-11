using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;

namespace puckipedia.bunq
{
    public class BunqEntitySerializer : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return true; 
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                reader.Read();
                var subType = objectType.GetElementType();
                var list = typeof(List<string>).GetGenericTypeDefinition().MakeGenericType(subType);
                IList arr = (IList)list.GetConstructor(new Type[] { }).Invoke(new object[] { });
                while (reader.TokenType != JsonToken.EndArray)
                {
                    arr.Add(ReadJson(reader, null, null, serializer));
                    reader.Read();
                }
                if (objectType.IsArray)
                    return list.GetMethod("ToArray").Invoke(arr, new object[] { });
                else
                    return list;
            }
            else if (reader.TokenType != JsonToken.StartObject)
                return null;

            reader.Read();
            var type = (string) reader.Value;
            reader.Read();
            var obj = serializer.Deserialize(reader, BunqEntityAttribute.Types[type]);
            reader.Read();

            return obj;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IList)
            {
                writer.WriteStartArray();
                foreach (var item in (IList) value)
                {
                    WriteJson(writer, item, serializer);
                }
                writer.WriteEndArray();
                return;
            }

            writer.WriteStartObject();
            writer.WritePropertyName(BunqEntityAttribute.ReverseTypes[value.GetType()]);
            serializer.Serialize(writer, value);
            writer.WriteEndObject();
        }
    }
}
