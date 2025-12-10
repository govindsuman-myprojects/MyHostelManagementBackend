using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;
    public LoginController(ILoginService loginService) => _loginService = loginService;

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var response = await _loginService.LoginAsync(dto);
        return Ok(response);
    }
}
