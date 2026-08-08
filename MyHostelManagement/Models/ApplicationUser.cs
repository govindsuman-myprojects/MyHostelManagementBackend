using Microsoft.AspNetCore.Identity;

namespace MyHostelManagement.Models;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public Guid? HostelId { get; set; } // optional
}
