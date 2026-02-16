using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Services.Implementations
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;

        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<ExpenseResponseDto> CreateAsync(CreateExpenseDto dto)
        {
            var expense = new Expense
            {
                HostelId = dto.HostelId,
                ExpenseCategoryId = dto.ExpenseCategoryId,
                ExpenseSubCategory = dto.ExpenseSubCategory,
                Amount = dto.Amount,
                ExpenseDate = dto.ExpenseDate
            };

            await _expenseRepository.CreateAsync(expense);
            return Map(expense);
        }

        public async Task<List<ExpenseResponseDto>> GetAsync(ExpenseFilterDto filter)
        {
            var expenses = await _expenseRepository.GetByFilterAsync(filter);
            return expenses.Select(Map).ToList();
        }

        private static ExpenseResponseDto Map(Expense expense)
        {
            return new ExpenseResponseDto
            {
                Id = expense.Id,
                HostelId = expense.HostelId,
                ExpenseCategoryId = expense.ExpenseCategoryId,
                ExpenseSubCategory = expense.ExpenseSubCategory,
                Amount = expense.Amount,
                CreatedAt = expense.CreatedAt,
                ExpenseDate = expense.ExpenseDate,
                ExpenseCategoryName = expense.ExpenseCategory?.CategoryName
            };
        }
    }
}
