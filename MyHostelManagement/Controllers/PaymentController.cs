using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.DTOs;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    // CREATE PAYMENT
    [HttpPost]
    public async Task<IActionResult> Create(CreatePaymentDto dto)
    {
        var result = await _paymentService.CreateAsync(dto);
        return Ok(result);
    }

    // GET PAYMENTS (FILTER)
    [HttpPost("search")]
    public async Task<IActionResult> Get(PaymentFilterDto filter)
    {
        return Ok(await _paymentService.GetAsync(filter));
    }

    // GET PAYMENTS (FILTER)
    [HttpPost("{hostelId}")]
    public async Task<IActionResult> GetByHostelId(Guid hostelId)
    {
        return Ok(await _paymentService.GetByHostelId(hostelId));
    }

    // GET Pending payments (FILTER)
    [HttpGet("pending/{hostelId}")]
    public async Task<IActionResult> GetPendingPayments(Guid hostelId)
    {
        return Ok(await _paymentService.GetPendingPayments(hostelId));
    }

    // GET Pending payments (FILTER)
    [HttpGet("recieved/{hostelId}")]
    public async Task<IActionResult> GetRecievedPayments(Guid hostelId)
    {
        return Ok(await _paymentService.GetRecievedPayments(hostelId));
    }

    // GET all tenants with paid / not-paid status for the current month
    [HttpGet("status/{hostelId}")]
    public async Task<IActionResult> GetTenantPaymentStatus(Guid hostelId)
    {
        return Ok(await _paymentService.GetTenantPaymentStatusAsync(hostelId));
    }
}

