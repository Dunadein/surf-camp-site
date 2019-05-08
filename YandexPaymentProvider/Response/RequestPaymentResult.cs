using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace YandexPaymentProvider.Response
{
    internal class RequestPaymentResult
    {
        [JsonProperty(PropertyName = "status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ResponseStatus Status { get; set; }

        [JsonProperty(PropertyName = "request_id")]
        public string RequestID { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "contract_amount")]
        public decimal WithdrawAmount { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "fees")]
        public Fee Fee { get; set; }
    }

    internal class Fee
    {
        [JsonProperty(PropertyName = "service")]
        public decimal Commission { get; set; }
    }
}
