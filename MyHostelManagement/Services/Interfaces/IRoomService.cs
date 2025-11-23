using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;

namespace MyHostelManagement.Api.Services.Interfaces;

public interface IRoomService
{
    Task<Room> CreateAsync(RoomDto dto);
    Task<IEnumerable<Room>> GetByHostelAsync(Guid hostelId);
}
