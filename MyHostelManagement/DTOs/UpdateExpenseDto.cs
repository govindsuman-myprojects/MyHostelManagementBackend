namespace MyHostelManagement.DTOs
{
    public class UpdateExpenseDto
    {
        public Guid ExpenseCategoryId { get; set; }
        public string? ExpenseSubCategory { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; } = DateTime.UtcNow;
        public string? PaymentMode { get; set; }
    }
}
