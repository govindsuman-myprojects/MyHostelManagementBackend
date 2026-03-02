using MyHostelManagement.Models;

namespace MyHostelManagement.Services.Interfaces
{
    public interface ISubscriptionService
    {
        Task<List<SubscriptionPlan>> GetPlans();
        Task<HostelSubscription?> GetCurrentSubscription(Guid hostelId);
        Task AddHostelSubscription(HostelSubscription subscriptionPlan);
        Task AddSubscriptionPayment(SubscriptionPayment payment);
        Task<List<SubscriptionPayment>> GetPaymentHistory(Guid hostelId);
    }
}
