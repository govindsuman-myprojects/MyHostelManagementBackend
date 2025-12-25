using AutoMapper;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Api.Repositories.Interfaces;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.DTOs;
using MyHostelManagement.Repositories.Interfaces;

namespace MyHostelManagement.Api.Services.Implementations;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepo;

    public RoomService(IRoomRepository roomRepo)
    {
        _roomRepo = roomRepo;
    }

    public async Task<Room> AddRoomAsync(RoomDto dto)
    {
        return await _roomRepo.AddRoomAsync(dto);
    }

    public async Task<IEnumerable<RoomResponseDto>> GetByHostelAsync(Guid hostelId)
    {
        return await _roomRepo.GetAllRoomAsync(hostelId);
    }

    public async Task<IEnumerable<RoomResponseUIDto>> GetAllRoomsByFloorAsync(Guid hostelId)
    {
        var response = await _roomRepo.GetAllRoomAsync(hostelId);
        return GetRoomsGroupedByFloor(response);
    }

    public async Task<RoomResponseDto?> GetRoomAsync(Guid roomId)
    {
        return await _roomRepo.GetRoomAsync(roomId);
    }

    public async Task<bool> UpdateRoomAsync(Guid roomId, RoomDto dto)
    {
        return await _roomRepo.UpdateRoomAsync(roomId, dto);
    }

    public async Task<bool> DeleteRoomAsync(Guid roomId)
    {
        return await _roomRepo.DeleteRoomAsync(roomId);
    }

    public IEnumerable<RoomResponseUIDto> GetRoomsGroupedByFloor(
        IEnumerable<RoomResponseDto> rooms)
    {
        return rooms
            .GroupBy(r => r.RoomFloor)
            .OrderBy(g => g.Key)
            .Select(floorGroup => new RoomResponseUIDto
            {
                Floor = floorGroup.Key,
                Rooms = floorGroup.Select(room => new RoomSummaryDto
                {
                    RoomId = room.Id,
                    RoomNumber = room.RoomNumber,
                    Capacity = room.Capacity,
                    Type = room.Type,
                    Rent = room.Rent,
                    OccupiedBeds = room.Beds.Count(b => b.Status == "occupied"),
                    VacantBeds = room.Capacity - room.Beds.Count(b => b.Status == "occupied")
                }).ToList()
            })
            .ToList();
    }
}
