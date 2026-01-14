using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using MyHostelManagement.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace MyHostelManagement.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IJwtTokenService _jwtService;

        public AuthService(
            IUserRepository userRepo,
            IRoleRepository roleRepo,
            IJwtTokenService jwtService)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByPhoneAsync(dto.PhoneNumber);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            using var hmac = new HMACSHA256(
                Convert.FromBase64String(user.PasswordSalt)
            );

            var computedHash = Convert.ToBase64String(
                hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password))
            );

            if (computedHash != user.PasswordHash)
                throw new UnauthorizedAccessException("Invalid credentials");

            var role = await _roleRepo.GetByIdAsync(user.RoleId);
            var token = _jwtService.GenerateToken(user, role.RoleName);

            return new AuthResponseDto
            {
                AccessToken = token,
                Role = role.RoleName,
                UserId = user.Id
            };
        }
    }
}
