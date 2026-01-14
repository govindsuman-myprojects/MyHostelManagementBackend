using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;

namespace MyHostelManagement.Repositories.Implementations
{
    public class ComplaintRepository : IComplaintRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ComplaintRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Complaint> CreateAsync(Complaint complaint)
        {
            _dbContext.Complaints.Add(complaint);
            await _dbContext.SaveChangesAsync();
            return complaint;
        }

        public async Task<Complaint?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Complaints
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Complaint>> GetByFilterAsync(ComplaintFilterDto filter)
        {
            var query = _dbContext.Complaints
                .Where(c => c.HostelId == filter.HostelId);

            if (filter.Status.HasValue)
                query = query.Where(c => c.Status == filter.Status);

            if (filter.CategoryId.HasValue)
                query = query.Where(c => c.CategoryId == filter.CategoryId);

            if (filter.UserId.HasValue)
                query = query.Where(c => c.UserId == filter.UserId);

            return await query
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(Complaint complaint)
        {
            complaint.UpdatedAt = DateTime.UtcNow;
            _dbContext.Complaints.Update(complaint);
            await _dbContext.SaveChangesAsync();
        }
    }
}
