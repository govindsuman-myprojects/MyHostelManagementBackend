using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.Api.DTOs;

namespace MyHostelManagement.Api.Controllers;

[ApiController]
[Route("api/hostels")]
public class HostelController : ControllerBase
{
    private readonly IHostelService _hostelService;

    public HostelController(IHostelService hostelService)
    {
        _hostelService = hostelService;
    }

    // ------------------------------------
    // CREATE HOSTEL
    // POST /api/hostels
    // ------------------------------------
    [HttpPost]
    public async Task<IActionResult> Create(CreateHostelDto dto)
    {
        var result = await _hostelService.CreateAsync(dto);
        return Ok(result);
    }

    // ------------------------------------
    // GET ALL HOSTELS
    // GET /api/hostels
    // ------------------------------------
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _hostelService.GetAllAsync();
        return Ok(result);
    }

    // ------------------------------------
    // GET HOSTEL BY ID
    // GET /api/hostels/{id}
    // ------------------------------------
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var hostel = await _hostelService.GetByIdAsync(id);
        if (hostel == null)
            return NotFound("Hostel not found");

        return Ok(hostel);
    }

    // ------------------------------------
    // UPDATE HOSTEL
    // PUT /api/hostels/{id}
    // ------------------------------------
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, CreateHostelDto dto)
    {
        var updated = await _hostelService.UpdateAsync(id, dto);
        if (!updated)
            return NotFound("Hostel not found");

        return Ok("Hostel updated successfully");
    }

    // ------------------------------------
    // DELETE HOSTEL
    // DELETE /api/hostels/{id}
    // ------------------------------------
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _hostelService.DeleteAsync(id);
        if (!deleted)
            return NotFound("Hostel not found");

        return Ok("Hostel deleted successfully");
    }
}

