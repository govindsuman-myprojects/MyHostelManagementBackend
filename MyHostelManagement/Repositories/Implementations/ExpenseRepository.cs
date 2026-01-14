using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
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
                .Where(e => e.HostelId == filter.HostelId);

            if (filter.ExpenseCategoryId.HasValue)
                query = query.Where(e => e.ExpenseCategoryId == filter.ExpenseCategoryId);

            if (filter.Month.HasValue)
                query = query.Where(e => e.CreatedAt.Month == filter.Month);

            if (filter.Year.HasValue)
                query = query.Where(e => e.CreatedAt.Year == filter.Year);

            return await query
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }
    }
}
