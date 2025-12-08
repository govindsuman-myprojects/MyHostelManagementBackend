using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.Api.DTOs;

namespace MyHostelManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HostelsController : ControllerBase
{
    private readonly IHostelService _hostelService;
    public HostelsController(IHostelService hostelService) => _hostelService = hostelService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] HostelDto dto)
    {
        var created = await _hostelService.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var h = await _hostelService.GetByIdAsync(id);
        if (h == null) return NotFound();
        return Ok(h);
    }

    [HttpGet]
    public async Task<IActionResult> List() => Ok(await _hostelService.GetAllAsync());

    [HttpGet("owner-dashboard/{id}")]
    public async Task<IActionResult> GetOwnerDashboard(Guid id)
    {
        var response = await _hostelService.GetOwnerDashboardAsync(id);
        return Ok(response);
    }
}
