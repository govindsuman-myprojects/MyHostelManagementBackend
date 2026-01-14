using MyHostelManagement.Api.Models;
using MyHostelManagement.Models;

namespace MyHostelManagement.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<List<User>> GetByHostelAsync(Guid hostelId);
        Task<List<User>> GetByRoleAsync(Guid hostelId, string roleName);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<Role?> GetUserRole(Guid roleId);
        Task<User?> GetByPhoneAsync(long phoneNumber);
        Task<Guid> GetHostelIdUsingUserId(Guid userId);
    }

}
