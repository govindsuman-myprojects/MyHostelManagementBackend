using MyHostelManagement.DTOs;

namespace MyHostelManagement.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> CreateAsync(CreateUserDto dto);
        Task<UserResponseDto?> GetByIdAsync(Guid id);
        Task<List<UserResponseDto>> GetByHostelAsync(Guid hostelId);
        Task<List<UserResponseDto>> GetTenantsAsync(Guid hostelId);
        Task<List<UserResponseDto>> GetOwnersAsync(Guid hostelId);
        Task<bool> UpdateAsync(Guid id, UpdateUserDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> AssignRoom(Guid userId, Guid roomId);
        Task<Guid> GetHostelIdUsingUserId(Guid userId);
        Task<bool> UpdatePasswordAsync(Guid id, string password);
    }

}
