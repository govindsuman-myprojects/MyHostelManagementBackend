using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Controllers
{
    [ApiController]
    [Route("api/announcement-types")]
    public class AnnouncementTypeController : ControllerBase
    {
        private readonly IAnnouncementTypeService _service;

        public AnnouncementTypeController(IAnnouncementTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            return Ok(await _service.SearchAsync(keyword));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
