using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace YandexPaymentProvider.Response
{
    internal class InstanceIdResult
    {
        [JsonProperty(PropertyName = "status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ResponseStatus Status { get; set; }

        [JsonProperty(PropertyName = "instance_id")]
        public string InstanceId { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }
    }
}
