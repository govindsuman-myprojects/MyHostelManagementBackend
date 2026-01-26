using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.DTOs;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Api.Controllers;

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
        try
        {
            var result = await _paymentService.CreateAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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
}

