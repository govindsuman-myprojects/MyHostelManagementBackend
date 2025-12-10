namespace MyHostelManagement.Api.Models;

public class Payment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    public Guid HostelId { get; set; }
    public decimal Amount { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public string? Mode { get; set; } // cash/upi/bank
    public string? ReferenceId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTime.Now;
}
