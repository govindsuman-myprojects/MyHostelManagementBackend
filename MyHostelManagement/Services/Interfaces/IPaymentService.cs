using MyHostelManagement.Api.DTOs;
using MyHostelManagement.DTOs;

namespace MyHostelManagement.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> CreateAsync(CreatePaymentDto dto);
        Task<List<PaymentResponseDto>> GetAsync(PaymentFilterDto filter);
        Task<List<PaymentResponseDto>> GetByHostelId(Guid hostelId);
        Task<List<PendingPaymentsDto>> GetPendingPayments(Guid hostelId);


    }

}
