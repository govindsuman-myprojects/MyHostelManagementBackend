using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using System;

namespace MyHostelManagement.Repositories.Implementations
{
    public class ComplaintCategoryRepository : IComplaintCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public ComplaintCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ComplaintCategory>> GetAllAsync()
        {
            return await _context.ComplaintCategories
                .Where(c => c.Status == 1)
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<List<ComplaintCategory>> SearchAsync(string keyword)
        {
            return await _context.ComplaintCategories
                .Where(c =>
                    c.Status == 1 &&
                    c.CategoryName.ToLower().Contains(keyword.ToLower()))
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<ComplaintCategory?> GetByIdAsync(Guid id)
        {
            return await _context.ComplaintCategories
                .FirstOrDefaultAsync(c => c.Id == id && c.Status == 1);
        }
    }
}
