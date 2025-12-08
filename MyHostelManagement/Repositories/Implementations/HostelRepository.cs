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

            var totalRooms = hostel.Rooms.Count;

            var occupiedRooms = hostel.Rooms
                .SelectMany(r => r.Beds)
                .Count(b => b.Status == "occupied");

            var vacantRooms = hostel.Rooms
                .SelectMany(r => r.Beds)
                .Count(b => b.Status == "available");

            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var toalPaymentsToday = await _dbContext.Payments
                .Where(p => p.HostelId == id && p.CreatedAt >= today && p.CreatedAt < tomorrow)
                .ToListAsync();

            var paymentsToday = toalPaymentsToday.Sum(x => x.Amount);

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
                    TotalRooms = totalRooms,
                    OccupiedRooms = occupiedRooms,
                    VacantRooms = vacantRooms,
                    PaymentsToday = paymentsToday
                }
            };

        }
    }
}
