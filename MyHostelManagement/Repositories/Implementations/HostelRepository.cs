using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Api.Repositories.Interfaces;
using MyHostelManagement.Repositories.Interfaces;
using System.Data;
using MyHostelManagement.Api.DTOs;

namespace MyHostelManagement.Repositories.Implementations
{
    public class HostelRepository : IHostelRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public HostelRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<OwnerDashboardResponse> GetOwnerDashboardAsync(Guid id)
        {
            var hostel = await _dbContext.Hostels
                                .Where(h => h.Id == id)
                                .Include(h => h.Rooms)
                                    .ThenInclude(r => r.Beds)
                                .FirstOrDefaultAsync();

            if (hostel == null)
                return new OwnerDashboardResponse();

            var totalBeds = hostel.Rooms.SelectMany(x => x.Beds).Count();
            
            var vacantBeds = hostel.Rooms
                                 .SelectMany(r => r.Beds)
                                 .Count(b => b.Status == "available");

            var occupiedBeds = hostel.Rooms
                                   .SelectMany(r => r.Beds)
                                   .Count(b => b.Status == "occupied");
            
            var toalPaymentsToday = await _dbContext.Payments
                .Where(p => p.HostelId == id && p.CreatedAt >= DateTime.Today.Date && p.CreatedAt < DateTime.Today.Date.AddDays(1))
                .ToListAsync();
            var paymentsToday = toalPaymentsToday.Sum(x => x.Amount);

            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);

            var paymentsThisMonthList = await _dbContext.Payments
                .Where(p => p.HostelId == id
                         && p.CreatedAt >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date
                         && p.CreatedAt < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).Date)
                .ToListAsync();

            var paymentsThisMonth = paymentsThisMonthList.Sum(x => x.Amount);

            var activeTenants = await _dbContext.Tenants
                .Where(t => t.HostelId == id && t.Status == "active")
                .ToListAsync();

            var expectedMonthlyRent = activeTenants.Sum(t => t.Rent);

            var pendingPaymentsThisMonth = expectedMonthlyRent - paymentsThisMonth;

            if (pendingPaymentsThisMonth < 0)
                pendingPaymentsThisMonth = 0;

            var totalRooms = hostel.Rooms.Count;

            var vacantRooms = hostel.Rooms
                                    .Count(room => !room.Beds.Any(bed => bed.Status == "occupied"));


            return new OwnerDashboardResponse
            {
                User = new UserDto
                {
                    FullName = hostel.OwnerName ?? string.Empty,
                },
                Hostel = new HostelDto
                {
                    Name = hostel.Name
                },
                Stats = new StatsDto
                {
                    TotalBeds = totalBeds,
                    VacantBeds = vacantBeds,
                    OccupiedBeds = occupiedBeds,
                    PaymentsToday = paymentsToday,
                    PaymentsRecievedThisMonth = 112000,
                    PaymentsPendingThisMonth = 12000,
                    TotalRooms = totalRooms,
                    VacantRooms = vacantRooms
                }
            };

        }
    }
}
