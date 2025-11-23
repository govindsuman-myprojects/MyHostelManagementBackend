using Microsoft.AspNetCore.Identity;

namespace MyHostelManagement.Api.Models;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public Guid? HostelId { get; set; } // optional
}
