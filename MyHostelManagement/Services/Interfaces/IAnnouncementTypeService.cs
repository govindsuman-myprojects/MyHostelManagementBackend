using MyHostelManagement.DTOs;

namespace MyHostelManagement.Services.Interfaces
{
    public interface IAnnouncementTypeService
    {
        Task<List<AnnouncementTypeResponseDto>> GetAllAsync();
        Task<List<AnnouncementTypeResponseDto>> SearchAsync(string keyword);
        Task<AnnouncementTypeResponseDto?> GetByIdAsync(Guid id);
    }
}
