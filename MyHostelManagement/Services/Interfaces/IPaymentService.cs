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
        Task<List<PendingPaymentsDto>> GetPendingPaymentsAsync(List<UserResponseDto> users, List<PaymentResponseDto> payments, List<RoomResponseDto> rooms);
        Task<List<PendingPaymentsDto>> GetRecievedPayments(Guid hostelId);

    }

}
