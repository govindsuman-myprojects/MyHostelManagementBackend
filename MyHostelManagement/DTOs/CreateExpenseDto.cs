namespace MyHostelManagement.DTOs
{
    public class CreateExpenseDto
    {
        public Guid HostelId { get; set; }
        public Guid ExpenseCategoryId { get; set; }
        public string? ExpenseSubCategory { get; set; }
        public decimal Amount { get; set; }
    }
}
