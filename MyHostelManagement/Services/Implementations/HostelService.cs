using AutoMapper;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.DTOs;
using MyHostelManagement.Repositories.Interfaces;

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
        var hostel = new Hostel
        {
            Name = dto.Name ?? string.Empty,
            Address = dto.Address,
            OwnerName = dto.OwnerName ?? string.Empty,
            PhoneNumber = dto.PhoneNumber,
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

        hostel.Name = dto.Name ?? string.Empty;
        hostel.Address = dto.Address;
        hostel.OwnerName = dto.OwnerName ?? string.Empty;
        hostel.PhoneNumber = dto.PhoneNumber;

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
