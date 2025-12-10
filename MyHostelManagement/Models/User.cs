using MyHostelManagement.Api.Models;

namespace MyHostelManagement.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public Guid HostelId { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? PasswordHash { get; set; }

        public string? Role { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
