using MyHostelManagement.Models;

namespace MyHostelManagement.Repositories.Interfaces
{
    public interface ITermsAndConditionsRepository
    {
        Task<TermsAndConditions> CreateAsync(TermsAndConditions terms);
        Task<List<TermsAndConditions>> GetAsync(Guid hostelId, Guid roleId);
        Task<TermsAndConditions?> GetByIdAsync(Guid id);
        Task UpdateAsync(TermsAndConditions terms);
    }
}
