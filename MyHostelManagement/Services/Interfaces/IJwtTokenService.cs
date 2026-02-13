using MyHostelManagement.Api.Models;
using MyHostelManagement.Models;

namespace MyHostelManagement.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user, string roleName);
        string GenerateToken(Hostel owner, string roleName);
    }
}
