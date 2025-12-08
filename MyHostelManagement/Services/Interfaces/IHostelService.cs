using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;

namespace MyHostelManagement.Api.Services.Interfaces;

public interface IHostelService
{
    Task<Hostel> CreateAsync(HostelDto dto);
    Task<Hostel?> GetByIdAsync(Guid id);
    Task<IEnumerable<Hostel>> GetAllAsync();
    Task<OwnerDashboardResponse> GetOwnerDashboardAsync(Guid id);
}