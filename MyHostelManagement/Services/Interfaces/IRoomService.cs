using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;

namespace MyHostelManagement.Api.Services.Interfaces;

public interface IRoomService
{
    Task<Room> AddRoomAsync(RoomDto dto);
    Task<IEnumerable<RoomResponseDto>> GetByHostelAsync(Guid hostelId);
    Task<RoomResponseDto?> GetRoomAsync(Guid roomId);
    Task<bool> UpdateRoomAsync(Guid roomId, RoomDto dto);
    Task<bool> DeleteRoomAsync(Guid roomId);
}
