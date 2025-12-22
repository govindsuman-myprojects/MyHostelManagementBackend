using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;

namespace MyHostelManagement.Repositories.Implementations
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LoginRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<User?> UserExistsOrNot(string emailOrPhone)
        {
            var response = await _dbContext.AppUsers
                                            .Where(x => x.Email == emailOrPhone || x.Phone == emailOrPhone).FirstOrDefaultAsync();
            return response;

        }

        public virtual async Task<User?> GetUserId(string emailOrPhone, string password)
        {
            var response = await _dbContext.AppUsers
                                .Where(x => (x.Email == emailOrPhone || x.Phone == emailOrPhone) && x.PasswordHash == password).FirstOrDefaultAsync();

            return response;
        }

        public virtual async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                _dbContext.AppUsers.Update(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
