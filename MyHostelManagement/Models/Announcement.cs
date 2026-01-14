using MyHostelManagement.Models.Common;

namespace MyHostelManagement.Models
{
    public class Announcement : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid TypeId { get; set; }
        public Guid HostelId { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Navigation
        public AnnouncementType Type { get; set; } = null!;
    }

}
