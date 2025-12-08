using MyHostelManagement.Api.Models;

namespace MyHostelManagement.Repositories.Interfaces
{
    public interface IHostelRepository
    {
        Task<OwnerDashboardResponse> GetOwnerDashboardAsync(Guid id);
    }
}
