using MyHostelManagement.Api.Models;
using MyHostelManagement.Models.Common;

namespace MyHostelManagement.Models
{
    public class Complaint : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid HostelId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoomId { get; set; }
        public int Status { get; set; }
        public Guid CategoryId { get; set; }
        public string? Content { get; set; }

        // Navigation
        public Hostel Hostel { get; set; } = null!;
        public User User { get; set; } = null!;
        public Room Room { get; set; } = null!;
        public ComplaintCategory Category { get; set; } = null!;
    }

}
