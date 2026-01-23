using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Repositories.Interfaces;

namespace MyHostelManagement.Repositories.Implementations
{
    public class RoomRepository : IRoomRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public RoomRepository(IMapper mapper, ApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _context = dbContext;
        }

        public async Task<Room> CreateAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<Room?> GetByIdAsync(Guid id)
        {
            return await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Room>> GetByHostelAsync(Guid hostelId, string status)
        {
            if (status.ToLower() == "vacant")
            {
                return await _context.Rooms
                    .Where(r => r.HostelId == hostelId && r.OccupiedBeds < r.TotalBeds)
                    .OrderBy(r => r.RoomNumber)
                    .ToListAsync();
            }
            else if (status.ToLower() == "occupied")
            {
                return await _context.Rooms
                    .Where(r => r.HostelId == hostelId && r.OccupiedBeds > 0)
                    .OrderBy(r => r.RoomNumber)
                    .ToListAsync();
            }
            else
            {
                return await _context.Rooms
                    .Where(r => r.HostelId == hostelId)
                    .OrderBy(r => r.RoomNumber)
                    .ToListAsync();
            }
        }


        public async Task UpdateAsync(Room room)
        {
            room.UpdatedAt = DateTime.UtcNow;
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Room room)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }
    }
}
