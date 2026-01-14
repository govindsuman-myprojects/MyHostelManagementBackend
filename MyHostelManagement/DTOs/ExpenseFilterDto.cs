namespace MyHostelManagement.DTOs
{
    public class ExpenseFilterDto
    {
        public Guid HostelId { get; set; }
        public Guid? ExpenseCategoryId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
