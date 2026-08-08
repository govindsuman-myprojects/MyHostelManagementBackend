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
                ExpenseDate = ToUtc(dto.ExpenseDate),
                PaymentMode = dto.PaymentMode,
                ReceiptDocument = dto.ReceiptDocument
            };

            await _expenseRepository.CreateAsync(expense);
            return Map(expense);
        }

        // Npgsql only accepts Utc or Unspecified DateTimes for `timestamp with time zone` columns.
        // The default JSON DateTime converter can hand us Kind=Local when the client sends an
        // explicit non-Z offset, which Npgsql rejects outright — normalize before it gets there.
        private static DateTime ToUtc(DateTime value) => value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };

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
                PaymentMode = expense.PaymentMode,
                ReceiptDocument = expense.ReceiptDocument,
                ExpenseCategoryName = expense.ExpenseCategory?.CategoryName
            };
        }
    }
}
