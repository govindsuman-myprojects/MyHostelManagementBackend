using MyHostelManagement.Api.Models;
using MyHostelManagement.Models.Common;

namespace MyHostelManagement.Models
{
    public class Expense : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid HostelId { get; set; }
        public Guid ExpenseCategoryId { get; set; }
        public string? ExpenseSubCategory { get; set; }
        public decimal Amount { get; set; }

        // Navigation
        public Hostel Hostel { get; set; } = null!;
        public ExpenseCategory ExpenseCategory { get; set; } = null!;
    }

}
