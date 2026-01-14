using MyHostelManagement.Api.Models;
using MyHostelManagement.Models.Common;

namespace MyHostelManagement.Models
{
    public class TermsAndConditions : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid HostelId { get; set; }
        public Guid RoleId { get; set; }
        public string Content { get; set; } = string.Empty;

        // Navigation
        public Hostel Hostel { get; set; } = null!;
        public Role Role { get; set; } = null!;
    }

}
