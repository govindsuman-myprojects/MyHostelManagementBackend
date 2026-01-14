using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;

        public RoleService(IRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<RoleResponseDto> CreateAsync(CreateRoleDto dto)
        {
            var existing = await _repository.GetByNameAsync(dto.RoleName);
            if (existing != null)
                throw new Exception("Role already exists");

            var role = new Role
            {
                RoleName = dto.RoleName
            };

            await _repository.CreateAsync(role);
            return Map(role);
        }

        public async Task<List<RoleResponseDto>> GetAllAsync()
        {
            var roles = await _repository.GetAllAsync();
            return roles.Select(Map).ToList();
        }

        public async Task<RoleResponseDto?> GetByIdAsync(Guid id)
        {
            var role = await _repository.GetByIdAsync(id);
            return role == null ? null : Map(role);
        }

        private static RoleResponseDto Map(Role role)
        {
            return new RoleResponseDto
            {
                Id = role.Id,
                RoleName = role.RoleName
            };
        }
    }
}
