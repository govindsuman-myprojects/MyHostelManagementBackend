using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Repositories.Interfaces;
using System.Data;

namespace MyHostelManagement.Repositories.Implementations
{
    public class HostelRepository : IHostelRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public HostelRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Hostel> CreateAsync(Hostel hostel)
        {
            _dbContext.Hostels.Add(hostel);
            await _dbContext.SaveChangesAsync();
            return hostel;
        }

        public async Task<List<Hostel>> GetAllAsync()
        {
            return await _dbContext.Hostels
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();
        }

        public async Task<Hostel?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Hostels
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task UpdateAsync(Hostel hostel)
        {
            hostel.UpdatedAt = DateTime.UtcNow;
            _dbContext.Hostels.Update(hostel);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Hostel hostel)
        {
            _dbContext.Hostels.Remove(hostel);
            await _dbContext.SaveChangesAsync();
        }
    }
}
