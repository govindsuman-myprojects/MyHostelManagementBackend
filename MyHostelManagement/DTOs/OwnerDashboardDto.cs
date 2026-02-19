using MyHostelManagement.DTOs;

namespace MyHostelManagement.DTOs
{
    public class OwnerDashboardDto
    {
        public string OwnerName { get; set; }
        public string HostelName { get; set; }
        public int TotalBeds { get; set; }
        public int OccupiedBeds { get; set; }
        public int VacantBeds { get; set; }

        public decimal TodayReceivedPayments { get; set; }
        public decimal MonthReceivedPayments { get; set; }
        public decimal MonthPendingPayments { get; set; }

        public int PendingComplaintCount { get; set; }
        public decimal MonthExpenses { get; set; }

        public List<PendingPaymentsDto> PendingPayments { get; set; }
        public List<PendingComplaintsDto> PendingComplaints { get; set; }
    }

    public class PendingPaymentsDto
    {
        public Guid UserId { get; set; }
        public string TenantName { get; set; }
        public string RoomNumber { get; set; }
        public DateTime RentDueDate { get; set; }
        public Decimal RentDueAmount { get; set; }
    }

    public class PendingComplaintsDto
    {
        public Guid id { get; set; }
        public string? TenantName { get; set; }
        public string? RoomNumber { get; set; }
        public string? ComplaintCategory { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
