using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;

namespace MyHostelManagement.Api.Services.Interfaces;

public interface IPaymentService
{
    Task<Payment> CreateAsync(PaymentDto dto);
    Task<IEnumerable<Payment>> GetByTenantAsync(Guid tenantId);
}
