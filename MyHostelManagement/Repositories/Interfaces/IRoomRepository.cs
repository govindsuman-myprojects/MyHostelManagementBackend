using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;

namespace MyHostelManagement.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room> AddRoomAsync(RoomDto dto);
        Task<IEnumerable<RoomResponseDto>> GetAllRoomAsync(Guid hostelId);
        Task<RoomResponseDto?> GetRoomAsync(Guid roomId);
        Task<bool> UpdateRoomAsync(Guid roomId, RoomDto dto);
        Task<bool> DeleteRoomAsync(Guid roomId);
    }
}
