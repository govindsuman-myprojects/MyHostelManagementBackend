namespace MyHostelManagement.DTOs
{
    public class TenantDashboardDto
    {
        public string TenantName { get; set; }
        public string HostelName { get; set; }
        public decimal RentDue { get; set; }
        public string RoomNumber { get; set; }
        public DateTime? RentDueDate { get; set; }
        public List<AnnouncementDto> Announcements { get; set; }
        public List<ComplaintDto> Complaints { get; set; }
        public string TermsAndConditions { get; set; }
    }

    public class AnnouncementDto
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public string AnnouncementType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class ComplaintDto
    {
        public string ComplaintType { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
