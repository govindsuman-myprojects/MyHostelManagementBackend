using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.DTOs;
using MyHostelManagement.Repositories.Implementations;
using MyHostelManagement.Repositories.Interfaces;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;

        public PaymentService(IPaymentRepository paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        public async Task<PaymentResponseDto> CreateAsync(CreatePaymentDto dto)
        {
            var exists = await _paymentRepo.ExistsAsync(
                dto.UserId, dto.PaymentMonth, dto.PaymentYear);

            if (exists)
                throw new Exception("Payment already exists for this month");

            var payment = new Payment
            {
                HostelId = dto.HostelId,
                UserId = dto.UserId,
                Amount = dto.Amount,
                PaymentMonth = dto.PaymentMonth,
                PaymentYear = dto.PaymentYear
            };

            await _paymentRepo.CreateAsync(payment);
            return Map(payment);
        }

        public async Task<List<PaymentResponseDto>> GetAsync(PaymentFilterDto filter)
        {
            var payments = await _paymentRepo.GetByFilterAsync(filter);
            return payments.Select(Map).ToList();
        }

        private static PaymentResponseDto Map(Payment payment)
        {
            return new PaymentResponseDto
            {
                Id = payment.Id,
                UserId = payment.UserId,
                Amount = payment.Amount,
                PaymentMonth = payment.PaymentMonth,
                PaymentYear = payment.PaymentYear,
                CreatedAt = payment.CreatedAt
            };
        }
    }
}
