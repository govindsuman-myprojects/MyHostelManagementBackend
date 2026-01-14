using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Controllers
{
    [ApiController]
    [Route("api/complaint-categories")]
    public class ComplaintCategoryController : ControllerBase
    {
        private readonly IComplaintCategoryService _service;

        public ComplaintCategoryController(IComplaintCategoryService service)
        {
            _service = service;
        }

        // GET ALL (Dropdown)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        // SEARCH
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            return Ok(await _service.SearchAsync(keyword));
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }
    }
}
