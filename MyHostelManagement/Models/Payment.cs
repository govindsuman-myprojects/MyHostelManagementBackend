using MyHostelManagement.Models;
using MyHostelManagement.Models.Common;

namespace MyHostelManagement.Api.Models;

public class Payment : BaseEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid HostelId { get; set; }
    public Hostel? Hostel { get; set; }

    public decimal Amount { get; set; }
    public int PaymentMonth { get; set; }
    public int PaymentYear { get; set; }
}

