using AutoMapper;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Api.Repositories.Interfaces;
using MyHostelManagement.Api.Services.Interfaces;
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
}
