using MyHostelManagement.DTOs;

namespace MyHostelManagement.Services.Interfaces
{
    public interface IExpenseCategoryService
    {
        Task<List<ExpenseCategoryResponseDto>> GetAllAsync();
        Task<List<ExpenseCategoryResponseDto>> SearchAsync(string keyword);
        Task<ExpenseCategoryResponseDto?> GetByIdAsync(Guid id);
    }
}
