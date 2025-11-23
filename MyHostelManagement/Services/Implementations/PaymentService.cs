using AutoMapper;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Api.Repositories.Interfaces;
using MyHostelManagement.Api.DTOs;

namespace MyHostelManagement.Api.Services.Implementations;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public PaymentService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Payment> CreateAsync(PaymentDto dto)
    {
        var payment = _mapper.Map<Payment>(dto);
        await _uow.Payments.AddAsync(payment);
        await _uow.SaveChangesAsync();
        return payment;
    }

    public async Task<IEnumerable<Payment>> GetByTenantAsync(Guid tenantId) =>
        await _uow.Payments.FindAsync(p => ((Payment)p).TenantId == tenantId);
}
