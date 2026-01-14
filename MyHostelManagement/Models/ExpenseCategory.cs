using MyHostelManagement.Models.Common;

namespace MyHostelManagement.Models
{
    public class ExpenseCategory : BaseEntity
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int Status { get; set; }

        // Navigation
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }

}
