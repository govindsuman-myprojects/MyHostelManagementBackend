using MyHostelManagement.Api.Models;

namespace MyHostelManagement.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid HostelId { get; set; }
        public Guid UserId { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }

        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Hostel Hostel { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
