using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;
    public LoginController(ILoginService loginService) => _loginService = loginService;

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var response = await _loginService.LoginAsync(dto);
        return Ok(response);
    }

    //[HttpPost]
    //[Route("forgot-password")]
    //public async Task<IActionResult> Login([FromBody] LoginDto dto)
    //{
    //    var response = await _loginService.LoginAsync(dto);
    //    return Ok(response);
    //}

    [HttpPost]
    [Route("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
    {
        var response = await _loginService.ResetPasswordAsync(dto);
        return Ok(response);
    }
}
