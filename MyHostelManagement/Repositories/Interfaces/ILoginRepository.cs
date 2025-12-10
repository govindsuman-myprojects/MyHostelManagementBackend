using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.DTOs;

namespace MyHostelManagement.Repositories.Interfaces
{
    public interface ILoginRepository
    {
        Task<LoginResponseDTO> LoginAsync(LoginDto dto);

    }
}
