using MyHostelManagement.DTOs;

namespace MyHostelManagement.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<ExpenseResponseDto> CreateAsync(CreateExpenseDto dto);
        Task<List<ExpenseResponseDto>> GetAsync(ExpenseFilterDto filter);
        Task<ExpenseResponseDto?> UpdateAsync(Guid id, Guid hostelId, UpdateExpenseDto dto);
        Task<bool> DeleteAsync(Guid id, Guid hostelId);
    }
}
