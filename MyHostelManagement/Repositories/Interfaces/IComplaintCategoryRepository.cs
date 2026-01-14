using MyHostelManagement.Models;

namespace MyHostelManagement.Repositories.Interfaces
{
    public interface IComplaintCategoryRepository
    {
        Task<List<ComplaintCategory>> GetAllAsync();
        Task<List<ComplaintCategory>> SearchAsync(string keyword);
        Task<ComplaintCategory?> GetByIdAsync(Guid id);
    }
}
