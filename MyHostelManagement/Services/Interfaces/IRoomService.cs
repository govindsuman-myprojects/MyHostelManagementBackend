using MyHostelManagement.Api.DTOs;
using MyHostelManagement.DTOs;

namespace MyHostelManagement.Services.Interfaces;

public interface IRoomService
{
    Task<RoomResponseDto> CreateAsync(CreateRoomDto dto);
    Task<RoomResponseDto?> GetByIdAsync(Guid id);
    Task<List<RoomResponseDto>> GetByHostelAsync(Guid hostelId);
    Task<bool> UpdateAsync(Guid id, UpdateRoomDto dto);
    Task<bool> DeleteAsync(Guid id);
}
