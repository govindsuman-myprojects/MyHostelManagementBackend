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
                StartDate = ToUtc(dto.StartDate),
                EndDate = ToUtc(dto.EndDate)
            };

            await _announcementRepository.CreateAsync(announcement);
            return Map(announcement);
        }

        // Npgsql only accepts Utc or Unspecified DateTimes for `timestamp with time zone` columns.
        // The default JSON DateTime converter can hand us Kind=Local when the client sends an
        // explicit non-Z offset, which Npgsql rejects outright — normalize before it gets there.
        private static DateTime? ToUtc(DateTime? value)
        {
            if (!value.HasValue) return null;
            return value.Value.Kind switch
            {
                DateTimeKind.Utc => value.Value,
                DateTimeKind.Local => value.Value.ToUniversalTime(),
                _ => DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
            };
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
            announcement.StartDate = ToUtc(dto.StartDate);
            announcement.EndDate = ToUtc(dto.EndDate);

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
