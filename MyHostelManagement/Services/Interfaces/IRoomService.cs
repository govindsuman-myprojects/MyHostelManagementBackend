using MyHostelManagement.DTOs;

namespace MyHostelManagement.Services.Interfaces;

public interface IRoomService
{
    Task<RoomResponseDto> CreateAsync(CreateRoomDto dto);
    Task<RoomResponseDto?> GetByIdAsync(Guid id);
    Task<List<RoomResponseDto>> GetByHostelAsync(Guid hostelId, string status);
    Task<RoomResponseDto?> UpdateAsync(Guid id, Guid hostelId, UpdateRoomDto dto);
    Task<bool> DeleteAsync(Guid id);
}
