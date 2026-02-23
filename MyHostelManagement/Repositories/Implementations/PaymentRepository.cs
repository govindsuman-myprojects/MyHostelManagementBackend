using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
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
                .Include(x => x.User)
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

        public async Task<List<Payment>> GetByHostelId(Guid hostelId)
        {
            return await _context.Payments
                .Include(x => x.User)
                .ToListAsync();
        }

        public async Task<List<PendingPaymentsDto>> GetPendingPayments(Guid hostelId)
        {
            var filterPaymentDto = new PaymentFilterDto
            {
                HostelId = hostelId,
            };
            var payments = await GetByHostelId(hostelId);
            var paidUserIds = payments
                    .Where(p => p.PaymentMonth == DateTime.UtcNow.Month &&
                                p.PaymentYear == DateTime.UtcNow.Year)
                    .Select(p => p.UserId)
                    .ToHashSet();

            var users =  await _context.Users
                                       .Include(u => u.Role)
                                       .Where(u => u.HostelId == hostelId &&
                                                   u.Role.RoleName == "Tenant" &&
                                                   u.Status == 1)
                                       .ToListAsync();

            var rooms = await _context.Rooms
                                       .Where(r => r.HostelId == hostelId)
                                       .OrderBy(r => r.RoomNumber)
                                       .ToListAsync();

            var usersWithoutPayments = users
                .Where(u => !paidUserIds.Contains(u.Id))
                .ToList();

            var pendingPayments = new List<PendingPaymentsDto>();
            foreach (var item in usersWithoutPayments)
            {
                var roomNumber = rooms.FirstOrDefault(r => r.Id == item.RoomId);
                var pendingPayemnt = new PendingPaymentsDto
                {
                    UserId = item.Id,
                    TenantName = item.Name,
                    RoomNumber = roomNumber?.RoomNumber ?? string.Empty,
                    RentDueDate = item.JoinDate.AddDays(-1),
                    RentDueAmount = (decimal)item.RentAmount
                };
                pendingPayments.Add(pendingPayemnt);
            }
            return pendingPayments;
        }
    }
}
