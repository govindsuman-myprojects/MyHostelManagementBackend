using MyHostelManagement.Models;

namespace MyHostelManagement.Repositories.Interfaces
{
    public interface IAnnouncementTypeRepository
    {
        Task<List<AnnouncementType>> GetAllAsync();
        Task<List<AnnouncementType>> SearchAsync(string keyword);
        Task<AnnouncementType?> GetByIdAsync(Guid id);
    }
}
