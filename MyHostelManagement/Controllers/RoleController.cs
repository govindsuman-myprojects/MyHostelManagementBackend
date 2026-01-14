using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.DTOs;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Controllers
{
    [ApiController]
    [Route("api/roles")]
    [Authorize(Roles = "Owner")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }

        // CREATE ROLE (OPTIONAL – use once)
        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleDto dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }

        // GET ALL ROLES
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        // GET ROLE BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var role = await _service.GetByIdAsync(id);
            if (role == null)
                return NotFound();

            return Ok(role);
        }
    }
}
