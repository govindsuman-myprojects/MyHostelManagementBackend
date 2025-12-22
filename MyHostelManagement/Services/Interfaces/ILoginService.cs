using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.DTOs;

namespace MyHostelManagement.Services.Interfaces
{
    public interface ILoginService
    {
        Task<LoginResponseDTO> LoginAsync(LoginDto dto);
        Task<LoginResponseDTO> ResetPasswordAsync(ResetPasswordDTO dto);

    }
}
