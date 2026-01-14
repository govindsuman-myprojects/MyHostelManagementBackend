namespace MyHostelManagement.DTOs
{
    public class TenantDashboardDto
    {
        public string TenantName { get; set; }
        public string HostelName { get; set; }
        public decimal RentDue { get; set; }
        public string RoomNumber { get; set; }
        public int PendingComplaints { get; set; }
        public string TermsAndConditions { get; set; }
        public DateTime? RentDueDate { get; set; }
    }
}
