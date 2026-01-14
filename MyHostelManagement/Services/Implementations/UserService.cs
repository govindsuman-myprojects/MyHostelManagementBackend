using AutoMapper;
using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using MyHostelManagement.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace MyHostelManagement.Api.Services.Implementations;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepo;
    private readonly IRoomRepository _roomRepo;

    public UserService(IMapper mapper, IUserRepository userRepo, IRoomRepository roomRepo)
    {
        _mapper = mapper;
        _userRepo = userRepo;
        _roomRepo = roomRepo;
    }

    public async Task<UserResponseDto> CreateAsync(CreateUserDto dto)
    {
        var role = await _userRepo.GetUserRole(dto.RoleId);
        if (role == null)
            throw new Exception("Invalid role");

        using var hmac = new HMACSHA256();
        var user = new User
        {
            HostelId = dto.HostelId,
            RoleId = dto.RoleId,
            Name = dto.Name,
            RentAmount = dto.RentAmount,
            RentCycle = dto.RentCycle,
            AdvanceAmount = dto.AdvanceAmount,
            PhoneNumber = dto.PhoneNumber,
            GurdianName = dto.GurdianName,
            GurdianPhoneNumber = dto.GurdianPhoneNumber,
            AadharCardNumber = dto.AadharCardNumber,
            AadharCardFile = dto.AadharCardFile,
            PasswordHash = Convert.ToBase64String(
                hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password))
            ),
            PasswordSalt = Convert.ToBase64String(hmac.Key),
            Status = 1
        };

        await _userRepo.CreateAsync(user);
        return Map(user, role.RoleName);
    }

    public async Task<UserResponseDto?> GetByIdAsync(Guid id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        return user == null ? null : Map(user, user.Role.RoleName);
    }

    public async Task<Guid> GetHostelIdUsingUserId(Guid userId)
    {
        return await _userRepo.GetHostelIdUsingUserId(userId);
    }

    public async Task<List<UserResponseDto>> GetByHostelAsync(Guid hostelId)
    {
        var users = await _userRepo.GetByHostelAsync(hostelId);
        return users.Select(u => Map(u, u.Role.RoleName)).ToList();
    }

    public async Task<List<UserResponseDto>> GetTenantsAsync(Guid hostelId)
    {
        var users = await _userRepo.GetByRoleAsync(hostelId, "Tenant");
        return users.Select(u => Map(u, "Tenant")).ToList();
    }

    public async Task<List<UserResponseDto>> GetOwnersAsync(Guid hostelId)
    {
        var users = await _userRepo.GetByRoleAsync(hostelId, "Owner");
        return users.Select(u => Map(u, "Owner")).ToList();
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null) return false;

        user.Name = dto.Name;
        user.RentAmount = dto.RentAmount;
        user.RentCycle = dto.RentCycle;
        user.AdvanceAmount = dto.AdvanceAmount;
        user.PhoneNumber = dto.PhoneNumber;
        user.GurdianName = dto.GurdianName;
        user.GurdianPhoneNumber = dto.GurdianPhoneNumber;
        user.Status = dto.Status;

        await _userRepo.UpdateAsync(user);
        return true;
    }

    public async Task<bool> AssignRoom(Guid userId, Guid roomId)
    {
        var room = await _roomRepo.GetByIdAsync(roomId);
        if (room == null) return false;
        else if (room.TotalBeds - room.OccupiedBeds <= 0)
        {
            return false;
        }
        else
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return false;

            user.RoomId = roomId;
            await _userRepo.UpdateAsync(user);

            //Update occipied bed count in rooms table
            room.OccupiedBeds = room.OccupiedBeds + 1;
            await _roomRepo.UpdateAsync(room); 
            return true;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        if (user == null) return false;

        await _userRepo.DeleteAsync(user);
        return true;
    }

    private static UserResponseDto Map(User user, string roleName)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            RoleName = roleName,
            RentAmount = user.RentAmount,
            RentCycle = user.RentCycle,
            PhoneNumber = user.PhoneNumber,
            Status = user.Status,
            HostelId = user.HostelId,
            RoomId = user.RoomId,
            JoiningDate = user.JoinDate
        };
    }
}
