namespace MyHostelManagement.Api.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidOn { get; set; }
    }
}
