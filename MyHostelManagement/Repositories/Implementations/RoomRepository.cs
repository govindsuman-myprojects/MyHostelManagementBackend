using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Repositories.Interfaces;

namespace MyHostelManagement.Repositories.Implementations
{
    public class RoomRepository : IRoomRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public RoomRepository(IMapper mapper, ApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public virtual async Task<Room> AddRoomAsync(RoomDto dto)
        {
            var room = _mapper.Map<Room>(dto);
            await _dbContext.Rooms.AddAsync(room);
            await _dbContext.SaveChangesAsync();
            return room;
        }

        public virtual async Task<IEnumerable<RoomResponseDto>> GetAllRoomAsync(Guid hostelId)
        {
            var rooms = await _dbContext.Rooms
                                .Include(x => x.Beds)
                                .Where(x => x.HostelId == hostelId).ToListAsync();

            var roomsResponseDto = _mapper.Map<List<RoomResponseDto>>(rooms);
            return roomsResponseDto;
        }

        public virtual async Task<RoomResponseDto?> GetRoomAsync(Guid roomId)
        {
            var response = await _dbContext.Rooms
                                .Include(x => x.Beds)
                                .Where(x => x.Id == roomId).FirstOrDefaultAsync();

            var roomResponseDto = _mapper.Map<RoomResponseDto>(response);
            return roomResponseDto;
        }

        public virtual async Task<bool> UpdateRoomAsync(Guid roomId, RoomDto dto)
        {
            var room = await _dbContext.Rooms
                                .Include(x => x.Beds)
                                .Where(x => x.Id == roomId).FirstOrDefaultAsync();
            if (room == null)
                return false;

            room.Id = roomId;
            room.RoomNumber = dto.RoomNumber;
            room.Capacity = dto.Capacity;
            room.Type = dto.Type;
            room.Rent = dto.Rent;
            room.RoomFloor = dto.RoomFloor;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> DeleteRoomAsync(Guid roomId)
        {
            var deleteRoom = await _dbContext.Rooms
                                .Include(x => x.Beds)
                                .Where(x => x.Id == roomId).FirstOrDefaultAsync();

            if (deleteRoom == null)
                return false;

            else if (deleteRoom.Beds.Any(x => x.Status == "occupied"))
                return false;
            
            else
            {
                _dbContext.Rooms.Remove(deleteRoom);
                await _dbContext.SaveChangesAsync();
                return true;
            }
        }
    }
}
