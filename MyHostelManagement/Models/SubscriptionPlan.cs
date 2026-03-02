namespace MyHostelManagement.Models
{
    public class SubscriptionPlan
    {
        public Guid Id { get; set; }
        public string PlanName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        public int IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<HostelSubscription> HostelSubscriptions { get; set; }
    }
}
