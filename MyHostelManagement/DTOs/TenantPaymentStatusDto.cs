namespace MyHostelManagement.DTOs
{
    public class TenantPaymentStatusDto
    {
        public Guid UserId { get; set; }
        public string TenantName { get; set; }
        public string RoomNumber { get; set; }
        public decimal RentAmount { get; set; }
        public DateTime RentDueDate { get; set; }
        public bool IsPaid { get; set; }
        public decimal AmountPaid { get; set; }
    }
}
