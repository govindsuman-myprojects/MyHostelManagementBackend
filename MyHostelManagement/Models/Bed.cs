namespace MyHostelManagement.Api.Models;

public class Bed
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RoomId { get; set; }
    public Room? Room { get; set; }
    public Guid HostelId { get; set; }
    public string? BedNumber { get; set; }
    public string Status { get; set; } = "available"; // available/occupied/reserved
}
