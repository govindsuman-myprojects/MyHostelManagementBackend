using MyHostelManagement.DTOs;

namespace MyHostelManagement.Services.Interfaces
{
    public interface IAnnouncementService
    {
        Task<AnnouncementResponseDto> CreateAsync(CreateAnnouncementDto dto);
        Task<List<AnnouncementResponseDto>> GetAsync(AnnouncementFilterDto filter);
        Task<bool> UpdateAsync(Guid id, UpdateAnnouncementDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
