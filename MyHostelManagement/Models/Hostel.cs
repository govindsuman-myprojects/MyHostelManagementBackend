using MyHostelManagement.Models;
using MyHostelManagement.Models.Common;

namespace MyHostelManagement.Api.Models;

public class Hostel : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty; public string? Address { get; set; }
    public string OwnerName { get; set; } = string.Empty; public long? PhoneNumber { get; set; }
    
    // Navigation
    public ICollection<User> Users { get; set; } = new List<User>(); 
    public ICollection<Room> Rooms { get; set; } = new List<Room>(); 
    public ICollection<Payment> Payments { get; set; } = new List<Payment>(); 
    public ICollection<Complaint> Complaints { get; set; } = new List<Complaint>(); 
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>(); 
    public ICollection<TermsAndConditions> TermsAndConditions { get; set; } = new List<TermsAndConditions>();
}
