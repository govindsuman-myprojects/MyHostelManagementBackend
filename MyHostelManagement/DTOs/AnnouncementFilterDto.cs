namespace MyHostelManagement.DTOs
{
    public class AnnouncementFilterDto
    {
        public Guid HostelId { get; set; }
        public bool OnlyActive { get; set; } = true;
    }
}
