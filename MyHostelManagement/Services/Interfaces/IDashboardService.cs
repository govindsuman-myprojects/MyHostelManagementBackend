using MyHostelManagement.Api.DTOs;
using MyHostelManagement.DTOs;

namespace MyHostelManagement.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<OwnerDashboardDto> GetOwnerDashboardAsync(Guid hostelId);
        Task<TenantDashboardDto> GetTenantDashboardAsync(Guid userId);
        Task<List<PendingPaymentsDto>> GetPendingPaymentsAsync(List<UserResponseDto> users, List<PaymentResponseDto> payments, List<RoomResponseDto> rooms);
    }
}
