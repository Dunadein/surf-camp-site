using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace YandexPaymentProvider.Response
{
    internal class StatusCheckResult
    {
        [JsonProperty(PropertyName = "status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ResponseStatus Status { get; set; }

        [JsonProperty(PropertyName = "next_retry")]
        public int NextRetry { get; set; }

        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        [JsonProperty(PropertyName = "invoice_id")]
        public string InvoiceId { get; set; }
    }
}
