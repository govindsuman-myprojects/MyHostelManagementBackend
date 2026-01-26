using MyHostelManagement.Models;

namespace MyHostelManagement.DTOs
{
    public class PaymentResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public int PaymentMonth { get; set; }
        public int PaymentYear { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TenantName { get; set; }
        public string RoomNumber { get; set; }
        public string PaymentMode { get; set; }
    }
}
