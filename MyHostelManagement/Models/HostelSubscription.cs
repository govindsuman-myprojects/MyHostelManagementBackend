namespace MyHostelManagement.Models
{
    public class HostelSubscription
    {
        public Guid Id { get; set; }

        public Guid HostelId { get; set; }
        public Guid UserId { get; set; }
        public Guid PlanId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public SubscriptionPlan Plan { get; set; }
        public ICollection<SubscriptionPayment> Payments { get; set; }
    }
}
