using MyHostelManagement.DTOs;

namespace MyHostelManagement.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<OwnerDashboardDto> GetOwnerDashboardAsync(Guid hostelId);
        Task<TenantDashboardDto> GetTenantDashboardAsync(Guid userId);
    }
}
