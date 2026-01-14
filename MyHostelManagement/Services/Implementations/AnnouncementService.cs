using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Services.Implementations
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public AnnouncementService(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public async Task<AnnouncementResponseDto> CreateAsync(CreateAnnouncementDto dto)
        {
            var announcement = new Announcement
            {
                HostelId = dto.HostelId,
                TypeId = dto.TypeId,
                Subject = dto.Subject,
                Message = dto.Message,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            await _announcementRepository.CreateAsync(announcement);
            return Map(announcement);
        }

        public async Task<List<AnnouncementResponseDto>> GetAsync(AnnouncementFilterDto filter)
        {
            var announcements = await _announcementRepository.GetByFilterAsync(filter);
            return announcements.Select(Map).ToList();
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateAnnouncementDto dto)
        {
            var announcement = await _announcementRepository.GetByIdAsync(id);
            if (announcement == null) return false;

            announcement.TypeId = dto.TypeId;
            announcement.Subject = dto.Subject;
            announcement.Message = dto.Message;
            announcement.StartDate = dto.StartDate;
            announcement.EndDate = dto.EndDate;

            await _announcementRepository.UpdateAsync(announcement);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var announcement = await _announcementRepository.GetByIdAsync(id);
            if (announcement == null) return false;

            await _announcementRepository.DeleteAsync(announcement);
            return true;
        }

        private static AnnouncementResponseDto Map(Announcement announcement)
        {
            return new AnnouncementResponseDto
            {
                Id = announcement.Id,
                HostelId = announcement.HostelId,
                TypeId = announcement.TypeId,
                Subject = announcement.Subject,
                Message = announcement.Message,
                StartDate = announcement.StartDate,
                EndDate = announcement.EndDate,
                CreatedAt = announcement.CreatedAt
            };
        }
    }
}
