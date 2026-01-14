using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Services.Implementations
{
    public class ExpenseCategoryService : IExpenseCategoryService
    {
        private readonly IExpenseCategoryRepository _repository;

        public ExpenseCategoryService(IExpenseCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ExpenseCategoryResponseDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            return categories.Select(Map).ToList();
        }

        public async Task<List<ExpenseCategoryResponseDto>> SearchAsync(string keyword)
        {
            var categories = await _repository.SearchAsync(keyword);
            return categories.Select(Map).ToList();
        }

        public async Task<ExpenseCategoryResponseDto?> GetByIdAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            return category == null ? null : Map(category);
        }

        private static ExpenseCategoryResponseDto Map(ExpenseCategory c)
        {
            return new ExpenseCategoryResponseDto
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                Status = c.Status
            };
        }
    }
}
