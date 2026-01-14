using MyHostelManagement.Models.Common;

namespace MyHostelManagement.Models
{
    public class AnnouncementType : BaseEntity
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public ICollection<Announcement> Announcements { get; set; } = new List<Announcement>();
    }

}
