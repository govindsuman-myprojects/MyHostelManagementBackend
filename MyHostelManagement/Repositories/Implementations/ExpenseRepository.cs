using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Data;
using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using System;

namespace MyHostelManagement.Repositories.Implementations
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ApplicationDbContext _context;

        public ExpenseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Expense> CreateAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<List<Expense>> GetByFilterAsync(ExpenseFilterDto filter)
        {
            var query = _context.Expenses
                .Include(e => e.ExpenseCategory)
                .Where(e => e.HostelId == filter.HostelId);

            if (filter.ExpenseCategoryId.HasValue)
                query = query.Where(e => e.ExpenseCategoryId == filter.ExpenseCategoryId);

            if (filter.Month.HasValue)
                query = query.Where(e => e.ExpenseDate.Month == filter.Month);

            if (filter.Year.HasValue)
                query = query.Where(e => e.ExpenseDate.Year == filter.Year);

            return await query
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<Expense?> GetByIdAsync(Guid id)
        {
            return await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task UpdateAsync(Expense expense)
        {
            expense.UpdatedAt = DateTime.UtcNow;
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Expense expense)
        {
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
        }
    }
}
