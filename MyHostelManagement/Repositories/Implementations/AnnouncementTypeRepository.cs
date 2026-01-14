using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using System;

namespace MyHostelManagement.Repositories.Implementations
{
    public class AnnouncementTypeRepository : IAnnouncementTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AnnouncementType>> GetAllAsync()
        {
            return await _context.AnnouncementTypes
                .Where(x => x.Status == 1)
                .OrderBy(x => x.TypeName)
                .ToListAsync();
        }

        public async Task<List<AnnouncementType>> SearchAsync(string keyword)
        {
            return await _context.AnnouncementTypes
                .Where(x =>
                    x.Status == 1 &&
                    x.TypeName.ToLower().Contains(keyword.ToLower()))
                .OrderBy(x => x.TypeName)
                .ToListAsync();
        }

        public async Task<AnnouncementType?> GetByIdAsync(Guid id)
        {
            return await _context.AnnouncementTypes
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == 1);
        }
    }
}
