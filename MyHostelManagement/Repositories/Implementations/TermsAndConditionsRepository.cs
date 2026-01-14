using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using System;

namespace MyHostelManagement.Repositories.Implementations
{
    public class TermsAndConditionsRepository : ITermsAndConditionsRepository
    {
        private readonly ApplicationDbContext _context;

        public TermsAndConditionsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TermsAndConditions> CreateAsync(TermsAndConditions terms)
        {
            _context.TermsAndConditions.Add(terms);
            await _context.SaveChangesAsync();
            return terms;
        }

        public async Task<TermsAndConditions?> GetAsync(Guid hostelId, Guid roleId)
        {
            return await _context.TermsAndConditions
                .FirstOrDefaultAsync(t =>
                    t.HostelId == hostelId);
        }

        public async Task<TermsAndConditions?> GetByIdAsync(Guid id)
        {
            return await _context.TermsAndConditions
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task UpdateAsync(TermsAndConditions terms)
        {
            terms.UpdatedAt = DateTime.UtcNow;
            _context.TermsAndConditions.Update(terms);
            await _context.SaveChangesAsync();
        }
    }
}
