namespace MyHostelManagement.DTOs
{
    public class CreateAnnouncementDto
    {
        public Guid HostelId { get; set; }
        public Guid TypeId { get; set; }

        public string? Subject { get; set; }
        public string? Message { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
