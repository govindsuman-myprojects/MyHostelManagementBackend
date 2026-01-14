using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.DTOs;
using MyHostelManagement.Services.Interfaces;

[ApiController]
[Route("api/users")]
// [Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // CREATE USER (OWNER / TENANT)
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        var result = await _userService.CreateAsync(dto);
        return Ok(result);
    }

    // GET USER BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound("User not found");

        return Ok(user);
    }

    // GET USERS BY HOSTEL
    [HttpGet("hostel/{hostelId}")]
    public async Task<IActionResult> GetByHostel(Guid hostelId)
    {
        return Ok(await _userService.GetByHostelAsync(hostelId));
    }

    // GET TENANTS
    [HttpGet("hostel/{hostelId}/tenants")]
    public async Task<IActionResult> GetTenants(Guid hostelId)
    {
        return Ok(await _userService.GetTenantsAsync(hostelId));
    }

    // GET OWNERS
    [HttpGet("hostel/{hostelId}/owners")]
    public async Task<IActionResult> GetOwners(Guid hostelId)
    {
        return Ok(await _userService.GetOwnersAsync(hostelId));
    }

    // UPDATE USER
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateUserDto dto)
    {
        var updated = await _userService.UpdateAsync(id, dto);
        if (!updated)
            return NotFound("User not found");

        return Ok("User updated successfully");
    }

    // Assign User to Room
    [HttpPut("assign-room/{userId}/{roomId}")]
    public async Task<IActionResult> AssignRoom(Guid userId, Guid roomId)
    {
        var updated = await _userService.AssignRoom(userId, roomId);
        if (!updated)
            return NotFound("User not found");

        return Ok("User updated successfully");
    }

    // DELETE USER
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _userService.DeleteAsync(id);
        if (!deleted)
            return NotFound("User not found");

        return Ok("User deleted successfully");
    }
}
