using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    [Authorize]
    public class DashboardController :ControllerBase
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        // OWNER DASHBOARD
        [HttpGet("owner")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> OwnerDashboard()
        {
            var hostelId = Guid.Parse(User.FindFirst("hostelId")!.Value);
            return Ok(await _service.GetOwnerDashboardAsync(hostelId));
        }

        // TENANT DASHBOARD
        [HttpGet("tenant")]
        [Authorize(Roles = "Tenant")]
        public async Task<IActionResult> TenantDashboard()
        {
            var userId = Guid.Parse(User.FindFirst("userId")!.Value);
            return Ok(await _service.GetTenantDashboardAsync(userId));
        }
    }
}
