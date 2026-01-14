using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Repositories.Interfaces;

namespace MyHostelManagement.Repositories.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<List<Payment>> GetByFilterAsync(PaymentFilterDto filter)
        {
            var query = _context.Payments.AsQueryable();

            query = query.Where(p => p.HostelId == filter.HostelId);

            if (filter.UserId.HasValue)
                query = query.Where(p => p.UserId == filter.UserId);

            if (filter.Month.HasValue)
                query = query.Where(p => p.PaymentMonth == filter.Month);

            if (filter.Year.HasValue)
                query = query.Where(p => p.PaymentYear == filter.Year);

            return await query
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid userId, int month, int year)
        {
            return await _context.Payments.AnyAsync(p =>
                p.UserId == userId &&
                p.PaymentMonth == month &&
                p.PaymentYear == year);
        }
    }
}
