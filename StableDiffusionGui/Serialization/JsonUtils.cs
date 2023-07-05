using Newtonsoft.Json;
using StableDiffusionGui.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StableDiffusionGui.Serialization
{
    internal class JsonUtils
    {
        public class TolerantEnumConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                Type type = IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
                return type.IsEnum;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                bool isNullable = IsNullableType(objectType);
                Type enumType = isNullable ? Nullable.GetUnderlyingType(objectType) : objectType;

                string[] names = Enum.GetNames(enumType);

                if (reader.TokenType == JsonToken.String)
                {
                    string enumText = reader.Value.ToString();

                    if (!string.IsNullOrEmpty(enumText))
                    {
                        string match = names
                            .Where(n => string.Equals(n, enumText, StringComparison.OrdinalIgnoreCase))
                            .FirstOrDefault();

                        if (match != null)
                        {
                            return Enum.Parse(enumType, match);
                        }
                    }
                }
                else if (reader.TokenType == JsonToken.Integer)
                {
                    int enumVal = Convert.ToInt32(reader.Value);
                    int[] values = (int[])Enum.GetValues(enumType);
                    if (values.Contains(enumVal))
                    {
                        return Enum.Parse(enumType, enumVal.ToString());
                    }
                }

                if (!isNullable)
                {
                    string defaultName = names
                        .Where(n => string.Equals(n, "Unknown", StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();

                    if (defaultName == null)
                    {
                        defaultName = names.First();
                    }

                    return Enum.Parse(enumType, defaultName);
                }

                return null;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }

            private bool IsNullableType(Type t)
            {
                return (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
            }
        }

        public class NullToEmptyStringConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(string);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                {
                    return string.Empty;
                }

                return (string)reader.Value;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }

        public class SingleValueToListConverter<T> : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(List<T>);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.StartArray)
                {
                    // If it's an array, let JSON.NET handle it with default behavior
                    return serializer.Deserialize(reader, objectType);
                }
                else
                {
                    // Otherwise, it's a single value; deserialize it and add to a list
                    T item = serializer.Deserialize<T>(reader);
                    return new List<T> { item };
                }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                // Let JSON.NET handle the serialization with default behavior
                serializer.Serialize(writer, value);
            }
        }

        public class EasyDictValueToListConverter<TKey, TValue> : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(EasyDict<TKey, List<TValue>>);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var result = new EasyDict<TKey, List<TValue>>();

                if (reader.TokenType == JsonToken.StartObject)
                {
                    // Load the JSON object into a JObject
                    var jObject = Newtonsoft.Json.Linq.JObject.Load(reader);

                    foreach (var prop in jObject.Properties())
                    {
                        TKey key = (TKey)Convert.ChangeType(prop.Name, typeof(TKey));
                        List<TValue> values;

                        // Check if the property value is an array
                        if (prop.Value.Type == Newtonsoft.Json.Linq.JTokenType.Array)
                        {
                            // Deserialize as a list
                            values = prop.Value.ToObject<List<TValue>>();
                        }
                        else
                        {
                            // Deserialize as a single value and add to a list
                            TValue value = prop.Value.ToObject<TValue>();
                            values = new List<TValue> { value };
                        }

                        result.Add(key, values);
                    }
                }

                return result;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                // Let JSON.NET handle the serialization with default behavior
                serializer.Serialize(writer, value);
            }
        }
    }
}
