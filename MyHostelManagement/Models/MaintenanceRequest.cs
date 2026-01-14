using MyHostelManagement.Models.Common;

namespace MyHostelManagement.Api.Models;

public class MaintenanceRequest : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid HostelId { get; set; }
    public Guid? RoomId { get; set; }
    public Guid? TenantId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = "pending";
}
