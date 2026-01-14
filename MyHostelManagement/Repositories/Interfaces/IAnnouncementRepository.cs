using MyHostelManagement.DTOs;
using MyHostelManagement.Models;

namespace MyHostelManagement.Repositories.Interfaces
{
    public interface IAnnouncementRepository
    {
        Task<Announcement> CreateAsync(Announcement announcement);
        Task<Announcement?> GetByIdAsync(Guid id);
        Task<List<Announcement>> GetByFilterAsync(AnnouncementFilterDto filter);
        Task UpdateAsync(Announcement announcement);
        Task DeleteAsync(Announcement announcement);
    }
}
