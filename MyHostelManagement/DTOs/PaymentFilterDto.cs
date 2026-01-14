namespace MyHostelManagement.Api.DTOs
{
    public class PaymentFilterDto
    {
        public Guid HostelId { get; set; }
        public Guid? UserId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }

}
