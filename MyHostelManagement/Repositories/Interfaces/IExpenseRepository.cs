using MyHostelManagement.DTOs;
using MyHostelManagement.Models;

namespace MyHostelManagement.Repositories.Interfaces
{
    public interface IExpenseRepository
    {
        Task<Expense> CreateAsync(Expense expense);
        Task<List<Expense>> GetByFilterAsync(ExpenseFilterDto filter);
    }
}
