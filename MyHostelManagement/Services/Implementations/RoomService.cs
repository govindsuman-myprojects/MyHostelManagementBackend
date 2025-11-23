using AutoMapper;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Api.Repositories.Interfaces;
using MyHostelManagement.Api.Services.Interfaces;

namespace MyHostelManagement.Api.Services.Implementations;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public RoomService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Room> CreateAsync(RoomDto dto)
    {
        var room = _mapper.Map<Room>(dto);
        await _uow.Rooms.AddAsync(room);
        await _uow.SaveChangesAsync();
        return room;
    }

    public async Task<IEnumerable<Room>> GetByHostelAsync(Guid hostelId)
    {
        var rooms = await _uow.Rooms.FindAsync(r => ((Room)r).HostelId == hostelId);
        return rooms;
    }
}
