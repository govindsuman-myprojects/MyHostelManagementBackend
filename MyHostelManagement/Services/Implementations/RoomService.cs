using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Services.Implementations;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepo;

    public RoomService(IRoomRepository roomRepo)
    {
        _roomRepo = roomRepo;
    }

    public async Task<RoomResponseDto> CreateAsync(CreateRoomDto dto)
    {
        var room = new Room
        {
            HostelId = dto.HostelId,
            RoomNumber = dto.RoomNumber,
            TotalBeds = dto.TotalBeds,
            OccupiedBeds = 0,
            Rent = dto.Rent,
            Type = dto.Type
        };

        await _roomRepo.CreateAsync(room);
        return Map(room);
    }

    public async Task<RoomResponseDto?> GetByIdAsync(Guid id)
    {
        var room = await _roomRepo.GetByIdAsync(id);
        return room == null ? null : Map(room);
    }

    public async Task<List<RoomResponseDto>> GetByHostelAsync(Guid hostelId, string status)
    {
        var rooms = await _roomRepo.GetByHostelAsync(hostelId, status);
        return rooms.Select(Map).ToList();
    }

    public async Task<RoomResponseDto?> UpdateAsync(Guid id, Guid hostelId, UpdateRoomDto dto)
    {
        var room = await _roomRepo.GetByIdAsync(id);
        if (room == null || room.HostelId != hostelId)
            return null;

        if (dto.TotalBeds < room.OccupiedBeds)
        {
            throw new ApiException(
                $"Cannot set total beds to {dto.TotalBeds}; room currently has {room.OccupiedBeds} occupied bed(s).",
                StatusCodes.Status400BadRequest);
        }

        room.RoomNumber = dto.RoomNumber;
        room.TotalBeds = dto.TotalBeds;
        room.Rent = dto.Rent;
        room.Type = dto.Type;

        await _roomRepo.UpdateAsync(room);
        return Map(room);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var room = await _roomRepo.GetByIdAsync(id);
        if (room == null) return false;

        await _roomRepo.DeleteAsync(room);
        return true;
    }

    private static RoomResponseDto Map(Room room)
    {
        return new RoomResponseDto
        {
            Id = room.Id,
            RoomNumber = room.RoomNumber ?? string.Empty,
            TotalBeds = room.TotalBeds,
            OccupiedBeds = room.OccupiedBeds,
            Rent = room.Rent,
            Type = room.Type
        };
    }
}
