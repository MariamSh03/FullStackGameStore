using AdminPanel.Bll.Constants;
using AdminPanel.Bll.DTOs.Authentification;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Web.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Web.Controllers;

[ApiController]
[Route("roles")]
public class RoleController : ControllerBase
{
    private readonly IAuthService _authService;

    public RoleController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    [RequirePermission(Permissions.ViewRoles)]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _authService.GetAllRolesAsync();
        return Ok(roles);
    }

    [HttpGet("{id}")]
    [RequirePermission(Permissions.ViewRoles)]
    public async Task<IActionResult> GetRoleById(string id)
    {
        var role = await _authService.GetRoleByIdAsync(id);
        return Ok(role);
    }

    [HttpDelete("{id}")]
    [RequirePermission(Permissions.ManageRoles)]
    public async Task<IActionResult> DeleteRoleById(string id)
    {
        await _authService.DeleteRoleByIdAsync(id);
        return Ok(new { message = "Role deleted successfully." });
    }

    [HttpGet("permissions")]
    [RequirePermission(Permissions.ViewRoles)]
    public async Task<IActionResult> GetAllPermissions()
    {
        var permissions = await _authService.GetAllPermissionsAsync();
        return Ok(permissions);
    }

    [HttpGet("{id}/permissions")]
    [RequirePermission(Permissions.ViewRoles)]
    public async Task<IActionResult> GetRolePermissions(string id)
    {
        var permissions = await _authService.GetRolePermissionsAsync(id);
        return Ok(permissions);
    }

    [HttpPost]
    [RequirePermission(Permissions.ManageRoles)]
    public async Task<IActionResult> AddRole([FromBody] AddRoleRequestDto dto)
    {
        var role = await _authService.AddRoleAsync(dto);
        return Ok(role);
    }

    [HttpPut]
    [RequirePermission(Permissions.ManageRoles)]
    public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleRequestDto dto)
    {
        var role = await _authService.UpdateRoleAsync(dto);
        return Ok(role);
    }
}