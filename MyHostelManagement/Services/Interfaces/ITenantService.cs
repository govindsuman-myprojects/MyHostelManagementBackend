using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;

namespace MyHostelManagement.Api.Services.Interfaces;

public interface ITenantService
{
    Task<Tenant> AddTenantAsync(TenantDto dto);
    Task<IEnumerable<Tenant>> GetByHostelAsync(Guid hostelId);
    Task<Tenant?> GetByIdAsync(Guid id);
    Task MoveAsync(Guid tenantId, Guid newRoomId, Guid newBedId);
    Task VacateAsync(Guid tenantId);
}
