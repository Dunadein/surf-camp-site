namespace YandexPaymentProvider.DTO
{
    public class PaymentStatusResult
    {
        public YandexStatus Status { get; set; }
        public int? RetrySeconds { get; set; }
        public string InvoiceId { get; set; }
    }

    public enum YandexStatus
    {
        Refused = 0,
        Success = 1,
        NeedRetry = 2
    }
}
