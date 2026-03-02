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
        private readonly ISubscriptionService _subscriptionService;

        public AuthService(
            IUserRepository userRepo,
            IRoleRepository roleRepo,
            IJwtTokenService jwtService,
            IHostelRepository hostelRepo,
            ISubscriptionService subscriptionService)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _jwtService = jwtService;
            _hostelRepo = hostelRepo;
            _subscriptionService = subscriptionService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByPhoneAsync(dto.PhoneNumber);
            if (user == null)
                throw new ApiException("Invalid credentials", 401);

            using var hmac = new HMACSHA256(
                    Convert.FromBase64String(user.PasswordSalt)
                );

            var computedHash = Convert.ToBase64String(
                hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password))
            );

            if (computedHash != user.PasswordHash)
                throw new ApiException("Invalid credentials", 401);

            var role = await _roleRepo.GetByIdAsync(user.RoleId);
            if (role == null)
                throw new ApiException("User role not found. Contact administrator.", 403);
            var token = _jwtService.GenerateToken(user, role.RoleName);

            // check if owner has valid subscription active
            var isValidSubscription = await _subscriptionService.GetCurrentSubscription(user.HostelId);
            if (isValidSubscription == null)
                throw new ApiException("No active subscription found.", 403);

            if (isValidSubscription.EndDate < DateTime.UtcNow)
                throw new ApiException("Subscription expired. Please renew your plan.", 403);

            return new AuthResponseDto
            {
                AccessToken = token,
                Role = role.RoleName,
                UserId = user.Id,
                HostelId = user.HostelId
            };
        }

        public async Task IsPhoneNumberRegistered(string phoneNumber)
        {
            var user = await _userRepo.GetByPhoneAsync(phoneNumber);
            if (user == null)
                throw new InvalidOperationException("Phone number not found, Enter valid Phone Number");
        }
    }
}
