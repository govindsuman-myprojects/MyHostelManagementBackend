using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.DTOs;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Api.Controllers;

[ApiController]
[Route("api/rooms")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;
    public RoomsController(IRoomService roomService) => _roomService = roomService;

    // CREATE ROOM
    [HttpPost]
    public async Task<IActionResult> Create(CreateRoomDto dto)
    {
        var result = await _roomService.CreateAsync(dto);
        return Ok(result);
    }

    // GET ROOM BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var room = await _roomService.GetByIdAsync(id);
        if (room == null)
            return NotFound("Room not found");

        return Ok(room);
    }

    // GET ROOMS BY HOSTEL
    [HttpGet("hostel/{hostelId}/{status}")]
    public async Task<IActionResult> GetByHostel(Guid hostelId, string status)
    {
        return Ok(await _roomService.GetByHostelAsync(hostelId, status));
    }

    // UPDATE ROOM
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateRoomDto dto)
    {
        var updated = await _roomService.UpdateAsync(id, dto);
        if (!updated)
            return NotFound("Room not found");

        return Ok("Room updated successfully");
    }

    // DELETE ROOM
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _roomService.DeleteAsync(id);
        if (!deleted)
            return NotFound("Room not found");

        return Ok("Room deleted successfully");
    }
}
