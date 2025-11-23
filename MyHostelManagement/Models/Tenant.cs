namespace MyHostelManagement.Api.Models;

public class Tenant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid HostelId { get; set; }
    public Hostel? Hostel { get; set; }

    public Guid? RoomId { get; set; }
    public Room? Room { get; set; }

    public Guid? BedId { get; set; }
    public Bed? Bed { get; set; }

    public string FullName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? GuardianName { get; set; }
    public string? GuardianPhone { get; set; }
    public string? Aadhar { get; set; }
    public string? Address { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public decimal Rent { get; set; }
    public decimal Advance { get; set; }
    public string Status { get; set; } = "active";
    public Dictionary<string, object>? Extra { get; set; } // maps to jsonb
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
