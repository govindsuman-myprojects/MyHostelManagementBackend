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

        public int PendingComplaints { get; set; }
        public decimal MonthExpenses { get; set; }
    }
}
