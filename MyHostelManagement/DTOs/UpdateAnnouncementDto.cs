namespace MyHostelManagement.DTOs
{
    public class UpdateAnnouncementDto
    {
        public Guid TypeId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
