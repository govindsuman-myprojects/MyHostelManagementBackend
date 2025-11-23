namespace MyHostelManagement.Api.Models;

public class MaintenanceRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid HostelId { get; set; }
    public Guid? RoomId { get; set; }
    public Guid? TenantId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = "pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
