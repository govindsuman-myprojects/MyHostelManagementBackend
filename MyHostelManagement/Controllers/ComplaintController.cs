using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.DTOs;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Controllers
{
    [ApiController]
    [Route("api/complaints")]
    public class ComplaintController : ControllerBase
    {
        private readonly IComplaintService _complaintService;

        public ComplaintController(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        // CREATE COMPLAINT (Tenant)
        [HttpPost]
        public async Task<IActionResult> Create(CreateComplaintDto dto)
        {
            var result = await _complaintService.CreateAsync(dto);
            return Ok(result);
        }

        // GET COMPLAINTS (Owner)
        [HttpPost("search")]
        public async Task<IActionResult> Get(ComplaintFilterDto filter)
        {
            return Ok(await _complaintService.GetAsync(filter));
        }

        // UPDATE STATUS
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, UpdateComplaintStatusDto dto)
        {
            var updated = await _complaintService.UpdateStatusAsync(id, dto);
            if (!updated)
                return NotFound("Complaint not found");

            return Ok("Status updated");
        }
    }
}
