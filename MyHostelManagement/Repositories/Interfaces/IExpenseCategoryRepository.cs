using MyHostelManagement.Models;

namespace MyHostelManagement.Repositories.Interfaces
{
    public interface IExpenseCategoryRepository
    {
        Task<List<ExpenseCategory>> GetAllAsync();
        Task<List<ExpenseCategory>> SearchAsync(string keyword);
        Task<ExpenseCategory?> GetByIdAsync(Guid id);
    }
}
