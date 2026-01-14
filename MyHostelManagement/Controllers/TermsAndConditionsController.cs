using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.DTOs;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Controllers
{
    [ApiController]
    [Route("api/terms")]
    public class TermsAndConditionsController : ControllerBase
    {
        private readonly ITermsAndConditionsService _termsService;

        public TermsAndConditionsController(ITermsAndConditionsService termsService)
        {
            _termsService = termsService;
        }

        // CREATE / UPDATE TERMS (Owner)
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate(CreateTermsDto dto)
        {
            var result = await _termsService.CreateOrUpdateAsync(dto);
            return Ok(result);
        }

        // GET TERMS (Tenant / Owner)
        [HttpPost("get")]
        public async Task<IActionResult> Get(TermsFilterDto filter)
        {
            var result = await _termsService.GetAsync(filter);
            if (result == null)
                return NotFound("Terms not found");

            return Ok(result);
        }
    }
}
