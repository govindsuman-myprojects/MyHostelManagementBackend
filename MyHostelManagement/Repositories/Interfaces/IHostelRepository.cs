using MyHostelManagement.Api.Models;

namespace MyHostelManagement.Repositories.Interfaces
{
    public interface IHostelRepository
    {
        Task<Hostel> CreateAsync(Hostel hostel);
        Task<List<Hostel>> GetAllAsync();
        Task<Hostel?> GetByIdAsync(Guid id);
        Task UpdateAsync(Hostel hostel);
        Task DeleteAsync(Hostel hostel);
    }

}
