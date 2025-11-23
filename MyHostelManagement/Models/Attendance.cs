namespace MyHostelManagement.Api.Models;

public class Attendance
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    public Guid HostelId { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = "present";
}
