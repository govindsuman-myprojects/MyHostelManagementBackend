using MyHostelManagement.DTOs;

namespace MyHostelManagement.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task IsPhoneNumberRegistered(string phoneNumber);
    }
}
