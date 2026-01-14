using MyHostelManagement.DTOs;

namespace MyHostelManagement.Services.Interfaces
{
    public interface IComplaintCategoryService
    {
        Task<List<ComplaintCategoryResponseDto>> GetAllAsync();
        Task<List<ComplaintCategoryResponseDto>> SearchAsync(string keyword);
        Task<ComplaintCategoryResponseDto?> GetByIdAsync(Guid id);
    }
}
