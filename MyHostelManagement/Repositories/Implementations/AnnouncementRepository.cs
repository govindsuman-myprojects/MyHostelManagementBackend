using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using System;

namespace MyHostelManagement.Repositories.Implementations
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Announcement> CreateAsync(Announcement announcement)
        {
            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();
            return announcement;
        }

        public async Task<Announcement?> GetByIdAsync(Guid id)
        {
            return await _context.Announcements
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Announcement>> GetByFilterAsync(AnnouncementFilterDto filter)
        {
            var query = _context.Announcements
                .Where(a => a.HostelId == filter.HostelId);

            if (filter.OnlyActive)
            {
                var now = DateTime.UtcNow;
                query = query.Where(a =>
                    (!a.StartDate.HasValue || a.StartDate <= now) &&
                    (!a.EndDate.HasValue || a.EndDate >= now));
            }

            return await query
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(Announcement announcement)
        {
            announcement.UpdatedAt = DateTime.UtcNow;
            _context.Announcements.Update(announcement);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Announcement announcement)
        {
            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
        }
    }
}
