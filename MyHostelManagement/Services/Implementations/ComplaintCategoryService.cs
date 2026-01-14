using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Services.Implementations
{
    public class ComplaintCategoryService : IComplaintCategoryService
    {
        private readonly IComplaintCategoryRepository _repository;

        public ComplaintCategoryService(IComplaintCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ComplaintCategoryResponseDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            return categories.Select(Map).ToList();
        }

        public async Task<List<ComplaintCategoryResponseDto>> SearchAsync(string keyword)
        {
            var categories = await _repository.SearchAsync(keyword);
            return categories.Select(Map).ToList();
        }

        public async Task<ComplaintCategoryResponseDto?> GetByIdAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            return category == null ? null : Map(category);
        }

        private static ComplaintCategoryResponseDto Map(ComplaintCategory c)
        {
            return new ComplaintCategoryResponseDto
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                Status = c.Status
            };
        }
    }
}
