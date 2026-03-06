namespace MyHostelManagement.Models
{
    public class SubscriptionResponse
    {
        public Guid Id { get; set; }
        public string? PlanName { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DaysRemaining { get; set; }

    }
}
