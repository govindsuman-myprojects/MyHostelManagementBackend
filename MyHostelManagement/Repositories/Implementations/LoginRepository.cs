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
            if (dto.UserRoleDate == "Tenant")
            {
                var tenantResponse = await _dbContext.Tenants
                                      .Where(x => x.Phone == dto.Email).FirstOrDefaultAsync();

                if (tenantResponse != null)
                {
                    return new LoginResponseDTO
                    {
                        id = tenantResponse.Id,
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

            var response = await _dbContext.AppUsers
                                .Where((x => x.Email == dto.Email || x.Phone == dto.Email  && x.PasswordHash == dto.Password)).FirstOrDefaultAsync();

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
