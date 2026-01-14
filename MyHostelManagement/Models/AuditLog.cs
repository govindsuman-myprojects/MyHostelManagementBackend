using MyHostelManagement.Models.Common;

namespace MyHostelManagement.Api.Models;

public class AuditLog : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid? HostelId { get; set; }
    public string Action { get; set; } = null!;
    public string? UserId { get; set; }
    public Dictionary<string, object>? Details { get; set; } // jsonb
}
