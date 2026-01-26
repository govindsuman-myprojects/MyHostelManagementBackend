using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;

namespace MyHostelManagement.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> CreateAsync(Payment payment);
        Task<List<Payment>> GetByFilterAsync(PaymentFilterDto filter);
        Task<bool> ExistsAsync(Guid userId, int month, int year);
        Task<List<Payment>> GetByHostelId(Guid hostelId);
    }

}
