using MyHostelManagement.DTOs;
using MyHostelManagement.Models;

namespace MyHostelManagement.Services.Interfaces;

public interface IHostelService
{
    Task<HostelResponseDto> CreateAsync(CreateHostelDto dto);
    Task<List<HostelResponseDto>> GetAllAsync();
    Task<HostelResponseDto?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, CreateHostelDto dto);
    Task<bool> DeleteAsync(Guid id);
}