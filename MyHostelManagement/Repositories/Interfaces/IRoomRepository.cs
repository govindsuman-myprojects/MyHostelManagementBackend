using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;

namespace MyHostelManagement.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room> CreateAsync(Room room);
        Task<Room?> GetByIdAsync(Guid id);
        Task<List<Room>> GetByHostelAsync(Guid hostelId, string status);
        Task UpdateAsync(Room room);
        Task DeleteAsync(Room room);
    }
}
