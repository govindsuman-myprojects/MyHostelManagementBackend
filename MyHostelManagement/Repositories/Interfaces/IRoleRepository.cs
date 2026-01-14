using MyHostelManagement.Models;

namespace MyHostelManagement.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> CreateAsync(Role role);
        Task<List<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(Guid id);
        Task<Role?> GetByNameAsync(string roleName);
    }
}
