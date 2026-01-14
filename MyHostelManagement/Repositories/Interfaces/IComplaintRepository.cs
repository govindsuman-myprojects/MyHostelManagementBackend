using MyHostelManagement.DTOs;
using MyHostelManagement.Models;

namespace MyHostelManagement.Repositories.Interfaces
{
    public interface IComplaintRepository
    {
        Task<Complaint> CreateAsync(Complaint complaint);
        Task<Complaint?> GetByIdAsync(Guid id);
        Task<List<Complaint>> GetByFilterAsync(ComplaintFilterDto filter);
        Task UpdateAsync(Complaint complaint);
    }

}
