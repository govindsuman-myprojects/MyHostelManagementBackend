using MyHostelManagement.Models;
using MyHostelManagement.Models.Common;

namespace MyHostelManagement.Api.Models;

public class Room : BaseEntity
{
    public Guid Id { get; set; }

    public Guid HostelId { get; set; }
    public Hostel? Hostel { get; set; }

    public string? RoomNumber { get; set; }
    public int TotalBeds { get; set; }
    public int OccupiedBeds { get; set; }
    public decimal Rent { get; set; }
    public int Type { get; set; }

    public ICollection<Complaint> Complaints { get; set; }
}
