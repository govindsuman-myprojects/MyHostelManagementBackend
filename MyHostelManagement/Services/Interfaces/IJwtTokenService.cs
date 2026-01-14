using MyHostelManagement.Models;

namespace MyHostelManagement.Services.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user, string roleName);
    }
}
