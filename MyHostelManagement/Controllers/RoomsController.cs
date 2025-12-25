using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.Models;

namespace MyHostelManagement.Api.Controllers;

[ApiController]
[Route("api")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;
    public RoomsController(IRoomService roomService) => _roomService = roomService;

    [HttpPost("rooms")]
    public async Task<IActionResult> AddRoom(RoomDto room)
    {
        var response = await _roomService.AddRoomAsync(room);

        return Ok(response);
    }

    [HttpGet("rooms/{hostelid}")]
    public async Task<IActionResult> GetAllRooms(Guid hostelid)
    {
        var response = await _roomService.GetByHostelAsync(hostelid);
        return Ok(response);
    }

    [HttpGet("roomsbyfloor/{hostelid}")]
    public async Task<IActionResult> GetAllRoomsForUI(Guid hostelid)
    {
        var response = await _roomService.GetAllRoomsByFloorAsync(hostelid);
        return Ok(response);
    }

    [HttpGet("room/{roomId}")]
    public async Task<IActionResult> GetRoom(Guid roomId)
    {
        var response = await _roomService.GetRoomAsync(roomId);
        return Ok(response);
    }

    [HttpPut("room/{roomId}")]
    public async Task<IActionResult> UpdateRoom(Guid roomId, RoomDto room)
    {
        var response = await _roomService.UpdateRoomAsync(roomId, room);
        
        if (!response)
        {
            return NotFound(new ApiResponse<int>
            {
                Success = false,
                Message = "Room not found",
                ErrorCode = "ROOM_NOT_FOUND"
            });
        }

        return Ok(new ApiResponse<Guid>
        {
            Success = true,
            Message = "Room updated successfully",
            Data = new Guid()
        });
    }

    [HttpDelete("room/{roomId}")]
    public async Task<IActionResult> DeleteRoom(Guid roomId)
    {
        var response = await _roomService.DeleteRoomAsync(roomId);

        if (!response)
        {
            return NotFound(new ApiResponse<int>
            {
                Success = false,
                Message = "Room not found or Please delete all the teanats attched to the beds",
                ErrorCode = "ROOM_NOT_FOUND"
            });
        }

        return Ok(new ApiResponse<Guid>
        {
            Success = true,
            Message = "Room deleted successfully",
            Data = roomId
        });
    }
}
