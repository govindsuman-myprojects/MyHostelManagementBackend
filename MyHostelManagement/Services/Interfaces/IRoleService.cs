using MyHostelManagement.DTOs;

namespace MyHostelManagement.Services.Interfaces
{
    public interface IRoleService
    {
        Task<RoleResponseDto> CreateAsync(CreateRoleDto dto);
        Task<List<RoleResponseDto>> GetAllAsync();
        Task<RoleResponseDto?> GetByIdAsync(Guid id);
    }
}
