namespace MyHostelManagement.DTOs
{
    public class ExpenseResponseDto
    {
        public Guid Id { get; set; }
        public Guid HostelId { get; set; }
        public Guid ExpenseCategoryId { get; set; }
        public string? ExpenseCategoryName { get; set; }
        public string? ExpenseSubCategory { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpenseDate { get; set; }
    }

}
