using MyHostelManagement.Api.Models;
using MyHostelManagement.Enums;
using MyHostelManagement.Models.Common;

namespace MyHostelManagement.Models
{
    public class User : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid HostelId { get; set; }
        public Hostel? Hostel { get; set; }

        public Guid RoleId { get; set; }
        public Role? Role { get; set; }

        public Guid? RoomId { get; set; }
        public Room? Room { get; set; }

        public string? Name { get; set; }
        public decimal? RentAmount { get; set; }
        public int RentCycle { get; set; }
        public decimal? AdvanceAmount { get; set; }

        public string PhoneNumber { get; set; }
        public string? GurdianName { get; set; }
        public long? GurdianPhoneNumber { get; set; }

        public string? AadharCardNumber { get; set; }
        public string? AadharCardFile { get; set; }

        public string PasswordHash { get; set; } 
        public string? PasswordSalt { get; set; }

        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        public int Status { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Complaint> Complaints { get; set; }
    }
}
