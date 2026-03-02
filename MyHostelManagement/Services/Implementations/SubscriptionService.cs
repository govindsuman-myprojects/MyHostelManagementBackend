using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Models;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Services.Implementations
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SubscriptionPlan>> GetPlans()
        {
            return await _context.SubscriptionPlans
                .Where(p => p.IsActive == 1)
                .ToListAsync();
        }

        public async Task<HostelSubscription?> GetCurrentSubscription(Guid hostelId)
        {
            return await _context.HostelSubscriptions
                .Include(s => s.Plan)
                .Where(s => s.HostelId == hostelId && s.Status == "Active")
                .FirstOrDefaultAsync();
        }

        public async Task AddHostelSubscription(HostelSubscription subscriptionPlan)
        {
            _context.HostelSubscriptions.Add(subscriptionPlan);
            await _context.SaveChangesAsync();
        }

        public async Task AddSubscriptionPayment(SubscriptionPayment payment)
        {
            _context.SubscriptionPayments.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SubscriptionPayment>> GetPaymentHistory(Guid hostelId)
        {
            return await _context.SubscriptionPayments
                                 .Where(p => p.HostelId == hostelId)
                                 .OrderByDescending(p => p.PaymentDate)
                                 .ToListAsync();
        }
    }
}
