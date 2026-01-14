using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using System;

namespace MyHostelManagement.Repositories.Implementations
{
    public class ExpenseCategoryRepository : IExpenseCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public ExpenseCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExpenseCategory>> GetAllAsync()
        {
            return await _context.ExpenseCategories
                .Where(x => x.Status == 1)
                .OrderBy(x => x.CategoryName)
                .ToListAsync();
        }

        public async Task<List<ExpenseCategory>> SearchAsync(string keyword)
        {
            return await _context.ExpenseCategories
                .Where(x =>
                    x.Status == 1 &&
                    x.CategoryName.ToLower().Contains(keyword.ToLower()))
                .OrderBy(x => x.CategoryName)
                .ToListAsync();
        }

        public async Task<ExpenseCategory?> GetByIdAsync(Guid id)
        {
            return await _context.ExpenseCategories
                .FirstOrDefaultAsync(x => x.Id == id && x.Status == 1);
        }
    }
}
