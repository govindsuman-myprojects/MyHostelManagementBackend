namespace MyHostelManagement.DTOs
{
    public class CreatePaymentDto
    {
        public Guid UserId { get; set; }
        public Guid HostelId { get; set; }
        public decimal Amount { get; set; }
        public int PaymentMonth { get; set; }
        public int PaymentYear { get; set; }
    }

}
