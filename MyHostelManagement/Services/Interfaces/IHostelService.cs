using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.DTOs;

namespace MyHostelManagement.Api.Services.Interfaces;

public interface IHostelService
{
    Task<HostelResponseDto> CreateAsync(CreateHostelDto dto);
    Task<List<HostelResponseDto>> GetAllAsync();
    Task<HostelResponseDto?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, CreateHostelDto dto);
    Task<bool> DeleteAsync(Guid id);
}