namespace MyHostelManagement.DTOs
{
    public class AnnouncementResponseDto
    {
        public Guid Id { get; set; }
        public Guid HostelId { get; set; }
        public Guid TypeId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
