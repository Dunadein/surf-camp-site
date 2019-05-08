namespace SurfLevel.Contracts.Models.DTO
{
    public class PaymentStatus
    {
        public Status Status { get; set; }
        public int? RetryIn { get; set; }
    }

    public enum Status
    {
        Success = 0,
        Refused = 1,
        Unknown = 2
    }
}
