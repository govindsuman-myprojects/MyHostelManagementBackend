using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.Models;

namespace MyHostelManagement.Controllers
{
    [ApiController]
    [Route("api")]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;
        public TenantsController(ITenantService tenantService) => _tenantService = tenantService;

        [HttpPost("tenants")]
        public async Task<IActionResult> AddTenant(TenantDto tenant)
        {
            var response = await _tenantService.AddTenantAsync(tenant);

            return Ok(response);
        }

        //[HttpGet("rooms/{hostelid}")]
        //public async Task<IActionResult> GetAllRooms(Guid hostelid)
        //{
        //    var response = await _tenantService.GetByHostelAsync(hostelid);
        //    return Ok(response);
        //}

        //[HttpGet("room/{roomId}")]
        //public async Task<IActionResult> GetRoom(Guid roomId)
        //{
        //    var response = await _tenantService.GetRoomAsync(roomId);
        //    return Ok(response);
        //}

        //[HttpPut("room/{roomId}")]
        //public async Task<IActionResult> UpdateRoom(Guid roomId, RoomDto room)
        //{
        //    var response = await _tenantService.UpdateRoomAsync(roomId, room);

        //    if (!response)
        //    {
        //        return NotFound(new ApiResponse<int>
        //        {
        //            Success = false,
        //            Message = "Room not found",
        //            ErrorCode = "ROOM_NOT_FOUND"
        //        });
        //    }

        //    return Ok(new ApiResponse<Guid>
        //    {
        //        Success = true,
        //        Message = "Room updated successfully",
        //        Data = new Guid()
        //    });
        //}

        //[HttpDelete("room/{roomId}")]
        //public async Task<IActionResult> DeleteRoom(Guid roomId)
        //{
        //    var response = await _roomService.DeleteRoomAsync(roomId);

        //    if (!response)
        //    {
        //        return NotFound(new ApiResponse<int>
        //        {
        //            Success = false,
        //            Message = "Room not found or Please delete all the teanats attched to the beds",
        //            ErrorCode = "ROOM_NOT_FOUND"
        //        });
        //    }

        //    return Ok(new ApiResponse<Guid>
        //    {
        //        Success = true,
        //        Message = "Room deleted successfully",
        //        Data = roomId
        //    });
        //}
    }
}
