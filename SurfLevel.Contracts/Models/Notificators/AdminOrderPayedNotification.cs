namespace SurfLevel.Contracts.Models.Notificators
{
    public class AdminOrderPayedNotification : Notification
    {
        public decimal Amount { get; set; }
        public int OrderId { get; set; }
    }
}
