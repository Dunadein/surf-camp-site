using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace YandexPaymentProvider.Response
{
    internal class ProcessPaymentResult
    {
        [JsonProperty(PropertyName = "status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ResponseStatus Status { get; set; }

        [JsonProperty(PropertyName = "acs_uri")]
        public string BaseUri { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "acs_params")]
        [JsonConverter(typeof(DictionaryConverter))]
        public IDictionary<string, string> Parameters { get; set; }
    }

    internal class DictionaryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(Dictionary<string, object>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            if (token.Type == JTokenType.Object)
            {
                return token.ToObject(objectType);
            }

            throw new JsonSerializationException($"The type {objectType} can not be deserialized.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
