using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.DTOs;
using MyHostelManagement.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace MyHostelManagement.Api.Services.Implementations;

public class HostelService : IHostelService
{
    private readonly IMapper _mapper;
    private readonly IHostelRepository _hostelRepo;

    public HostelService(IMapper mapper, IHostelRepository hostelRepo)
    {
        _mapper = mapper;
        _hostelRepo = hostelRepo;
    }

    public async Task<HostelResponseDto> CreateAsync(CreateHostelDto dto)
    {
        using var hmac = new HMACSHA256();
        var hostel = new Hostel
        {
            Name = dto.Name ?? string.Empty,
            Address = dto.Address,
            OwnerName = dto.OwnerName ?? string.Empty,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = Convert.ToBase64String(
                hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password))
            ),
            PasswordSalt = Convert.ToBase64String(hmac.Key),
        };

        await _hostelRepo.CreateAsync(hostel);

        return MapToDto(hostel);
    }

    public async Task<List<HostelResponseDto>> GetAllAsync()
    {
        var hostels = await _hostelRepo.GetAllAsync();
        return hostels.Select(MapToDto).ToList();
    }

    public async Task<HostelResponseDto?> GetByIdAsync(Guid id)
    {
        var hostel = await _hostelRepo.GetByIdAsync(id);
        if (hostel == null) return null;

        return MapToDto(hostel);
    }

    public async Task<bool> UpdateAsync(Guid id, CreateHostelDto dto)
    {
        var hostel = await _hostelRepo.GetByIdAsync(id);
        if (hostel == null) return false;
        using var hmac = new HMACSHA256();
        hostel.Name = dto.Name ?? string.Empty;
        hostel.Address = dto.Address;
        hostel.OwnerName = dto.OwnerName ?? string.Empty;
        hostel.PhoneNumber = dto.PhoneNumber;
        if (dto.IsPasswordUpdated)
        {
            hostel.PasswordHash = Convert.ToBase64String(
                hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password))
            );
            hostel.PasswordSalt = Convert.ToBase64String(hmac.Key);
        }

        await _hostelRepo.UpdateAsync(hostel);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var hostel = await _hostelRepo.GetByIdAsync(id);
        if (hostel == null) return false;

        await _hostelRepo.DeleteAsync(hostel);
        return true;
    }

    private static HostelResponseDto MapToDto(Hostel hostel)
    {
        return new HostelResponseDto
        {
            Id = hostel.Id,
            Name = hostel.Name,
            Address = hostel.Address,
            OwnerName = hostel.OwnerName,
            PhoneNumber = hostel.PhoneNumber
        };
    }
}
