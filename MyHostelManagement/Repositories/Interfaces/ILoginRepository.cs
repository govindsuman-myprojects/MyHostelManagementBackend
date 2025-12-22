using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.DTOs;
using MyHostelManagement.Models;

namespace MyHostelManagement.Repositories.Interfaces
{
    public interface ILoginRepository
    {
        Task<User?> UserExistsOrNot(string emailOrPhone);
        Task<User?> GetUserId(string emailOrPhone, string password);
        Task<bool> UpdateUserAsync(User user);

    }
}
