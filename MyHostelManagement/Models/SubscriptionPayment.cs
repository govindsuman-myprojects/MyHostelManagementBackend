namespace MyHostelManagement.Models
{
    public class SubscriptionPayment
    {
        public Guid Id { get; set; }

        public Guid SubscriptionId { get; set; }
        public Guid HostelId { get; set; }
        public Guid UserId { get; set; }

        public decimal Amount { get; set; }
        public string? PaymentMode { get; set; }
        public string? TransactionId { get; set; }
        public string? PaymentStatus { get; set; }

        public DateTime PaymentDate { get; set; }

        public HostelSubscription Subscription { get; set; }
    }
}
