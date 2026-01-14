using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Services.Implementations
{
    public class AnnouncementTypeService : IAnnouncementTypeService
    {
        private readonly IAnnouncementTypeRepository _repository;

        public AnnouncementTypeService(IAnnouncementTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AnnouncementTypeResponseDto>> GetAllAsync()
        {
            var types = await _repository.GetAllAsync();
            return types.Select(Map).ToList();
        }

        public async Task<List<AnnouncementTypeResponseDto>> SearchAsync(string keyword)
        {
            var types = await _repository.SearchAsync(keyword);
            return types.Select(Map).ToList();
        }

        public async Task<AnnouncementTypeResponseDto?> GetByIdAsync(Guid id)
        {
            var type = await _repository.GetByIdAsync(id);
            return type == null ? null : Map(type);
        }

        private static AnnouncementTypeResponseDto Map(AnnouncementType t)
        {
            return new AnnouncementTypeResponseDto
            {
                Id = t.Id,
                TypeName = t.TypeName,
                Status = t.Status
            };
        }
    }
}
