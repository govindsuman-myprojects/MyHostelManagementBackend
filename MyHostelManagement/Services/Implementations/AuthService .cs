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
        private readonly IHostelRepository _hostelRepo;

        public AuthService(
            IUserRepository userRepo,
            IRoleRepository roleRepo,
            IJwtTokenService jwtService,
            IHostelRepository hostelRepo)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _jwtService = jwtService;
            _hostelRepo = hostelRepo;
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
            if (role == null)
                throw new UnauthorizedAccessException("User role not found, Contact administrator");
            var token = _jwtService.GenerateToken(user, role.RoleName);

            return new AuthResponseDto
            {
                AccessToken = token,
                Role = role.RoleName,
                UserId = user.Id,
                HostelId = user.HostelId
            };

            //if (dto != null && dto.Role.ToLower() == "Owner".ToLower())
            //{
            //    var owner = await _hostelRepo.GetAllAsync();
            //    var matchedOwner = owner.FirstOrDefault(h => h.PhoneNumber == dto.PhoneNumber);
            //    if (matchedOwner == null)
            //        throw new UnauthorizedAccessException("Invalid credentials");

            //    using var hmac1 = new HMACSHA256(
            //        Convert.FromBase64String(matchedOwner.PasswordSalt)
            //        );

            //    var computedHash1 = Convert.ToBase64String(
            //    hmac1.ComputeHash(Encoding.UTF8.GetBytes(dto.Password))
            //    );

            //    if (computedHash1 != matchedOwner.PasswordHash)
            //        throw new UnauthorizedAccessException("Invalid credentials");

            //    var token = _jwtService.GenerateToken(matchedOwner, "Owner");

            //    return new AuthResponseDto
            //    {
            //        AccessToken = token,
            //        Role = "Owner",
            //        HostelId = matchedOwner.Id
            //    };
            //}
            //else
            //{
            //    var user = await _userRepo.GetByPhoneAsync(dto.PhoneNumber);
            //    if (user == null)
            //        throw new UnauthorizedAccessException("Invalid credentials");

            //    using var hmac = new HMACSHA256(
            //        Convert.FromBase64String(user.PasswordSalt)
            //    );

            //    var computedHash = Convert.ToBase64String(
            //        hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password))
            //    );

            //    if (computedHash != user.PasswordHash)
            //        throw new UnauthorizedAccessException("Invalid credentials");

            //    var role = await _roleRepo.GetByIdAsync(user.RoleId);
            //    var token = _jwtService.GenerateToken(user, role.RoleName);

            //    return new AuthResponseDto
            //    {
            //        AccessToken = token,
            //        Role = role.RoleName,
            //        UserId = user.Id,
            //        HostelId = user.HostelId
            //    };
            //}
        }

        public async Task IsPhoneNumberRegistered(string phoneNumber)
        {
            var user = await _userRepo.GetByPhoneAsync(phoneNumber);
            if (user == null)
                throw new InvalidOperationException("Phone number not found, Enter valid Phone Number");
        }
    }
}
