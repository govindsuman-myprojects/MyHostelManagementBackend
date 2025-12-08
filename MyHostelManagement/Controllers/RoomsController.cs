using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.Api.DTOs;

namespace MyHostelManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;
    public RoomsController(IRoomService roomService) => _roomService = roomService;

    //[HttpPost]
    //public async Task<IActionResult> Create([FromBody] HostelDto dto)
    //{
    //    var created = await _hostelService.CreateAsync(dto);
    //    return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    //}

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var h = await _roomService.GetByHostelAsync(id);
        if (h == null) return NotFound();
        return Ok(h);
    }

    //[HttpGet]
    //public async Task<IActionResult> List() => Ok(await _roomService.GetAllAsync());

    
}
