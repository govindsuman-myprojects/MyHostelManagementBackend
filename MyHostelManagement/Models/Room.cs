namespace MyHostelManagement.Api.Models;

public class Room
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid HostelId { get; set; }
    public Hostel? Hostel { get; set; }
    public string RoomNumber { get; set; } = null!;
    public int Capacity { get; set; }
    public decimal? Rent { get; set; }
    public string Type { get; set; } = null!;
    public ICollection<Bed> Beds { get; set; } = new List<Bed>();
}
