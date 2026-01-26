using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using System.Data;

namespace MyHostelManagement.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Guid> GetHostelIdUsingUserId(Guid userId)
        {
            return await _context.Users.Where(x => x.Id == userId).Select(x => x.HostelId).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetByHostelAsync(Guid hostelId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.HostelId == hostelId)
                .ToListAsync();
        }

        public async Task<List<User>> GetByRoleAsync(Guid hostelId, string roleName)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.HostelId == hostelId &&
                            u.Role.RoleName == roleName &&
                            u.Status == 1)
                .ToListAsync();
        }

        public async Task UpdateAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Role?> GetUserRole(Guid roleId)
        {
            var response = await _context.Roles.Where(x => x.Id == roleId).FirstOrDefaultAsync();
            return response;
        }

        public async Task<User?> GetByPhoneAsync(string phoneNumber)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}
