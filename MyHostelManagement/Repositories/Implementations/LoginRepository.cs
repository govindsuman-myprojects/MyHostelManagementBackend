using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.DTOs;
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

        public virtual async Task<LoginResponseDTO> LoginAsync(LoginDto dto)
        {
            
            var response = await _dbContext.AppUsers
                                .Where(x => x.Email == dto.Email && x.PasswordHash == dto.Password).FirstOrDefaultAsync();

            if (response != null)
            {
                return new LoginResponseDTO
                {
                    id = response.HostelId,
                    message = null
                };
            }
            else
            {
                return new LoginResponseDTO
                {
                    id = new Guid(),
                    message = "User Not Found"
                };
            }
        }
    }
}
