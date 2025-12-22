using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.Models;
using MyHostelManagement.Services.Interfaces;


namespace MyHostelManagement.Controllers
{
    [ApiController]
    [Route("api")]
    public class BedsController : ControllerBase
    {
        private readonly IBedService _bedService;
        public BedsController(IBedService bedService) => _bedService = bedService;

        [HttpPost("beds")]
        public async Task<IActionResult> AddBed(BedDto bed)
        {
            var response = await _bedService.AddBedAsync(bed);

            return Ok(response);
        }

        [HttpGet("beds/{roomId}")]
        public async Task<IActionResult> GetAllBedsForRoom(Guid roomId)
        {
            var response = await _bedService.GetAllBedsByRoomIdAsync(roomId);
            return Ok(response);
        }

        [HttpGet("bed/{hostelId}")]
        public async Task<IActionResult> GetAllBedsForHostel([FromQuery] bool? occupied, Guid hostelId)
        {
            var response = await _bedService.GetAllBedsForHostelAsync(occupied, hostelId);
            return Ok(response);
        }

        [HttpPut("bed/{bedId}")]
        public async Task<IActionResult> UpdateBed(Guid bedId, BedDto bed)
        {
            var response = await _bedService.UpdateBedAsync(bedId, bed);

            if (!response)
            {
                return NotFound(new ApiResponse<int>
                {
                    Success = false,
                    Message = "Bed not found",
                    ErrorCode = "BED_NOT_FOUND"
                });
            }

            return Ok(new ApiResponse<Guid>
            {
                Success = true,
                Message = "Room updated successfully",
                Data = new Guid()
            });
        }

        [HttpDelete("bed/{bedId}")]
        public async Task<IActionResult> DeleteRoom(Guid bedId)
        {
            var response = await _bedService.DeleteBedAsync(bedId);

            if (!response)
            {
                return NotFound(new ApiResponse<int>
                {
                    Success = false,
                    Message = "Bed not found or Please delete Tenants attched to the bed",
                    ErrorCode = "ROOM_NOT_FOUND"
                });
            }

            return Ok(new ApiResponse<Guid>
            {
                Success = true,
                Message = "Room deleted successfully",
                Data = bedId
            });
        }
    }
}
