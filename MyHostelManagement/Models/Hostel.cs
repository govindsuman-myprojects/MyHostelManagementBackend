namespace MyHostelManagement.Api.Models;

public class Hostel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } 
    public string? Address { get; set; }
    public string? OwnerName { get; set; }
    public string? Phone { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}
