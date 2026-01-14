using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.DTOs;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Controllers
{
    [ApiController]
    [Route("api/announcements")]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        // CREATE ANNOUNCEMENT (Owner)
        [HttpPost]
        public async Task<IActionResult> Create(CreateAnnouncementDto dto)
        {
            var result = await _announcementService.CreateAsync(dto);
            return Ok(result);
        }

        // GET ANNOUNCEMENTS (Tenant / Owner)
        [HttpPost("search")]
        public async Task<IActionResult> Get(AnnouncementFilterDto filter)
        {
            return Ok(await _announcementService.GetAsync(filter));
        }

        // UPDATE ANNOUNCEMENT
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateAnnouncementDto dto)
        {
            var updated = await _announcementService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound("Announcement not found");

            return Ok("Announcement updated");
        }

        // DELETE ANNOUNCEMENT
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _announcementService.DeleteAsync(id);
            if (!deleted)
                return NotFound("Announcement not found");

            return Ok("Announcement deleted");
        }
    }
}
