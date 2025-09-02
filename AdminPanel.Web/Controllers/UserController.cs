using AdminPanel.Bll.Constants;
using AdminPanel.Bll.DTOs.Authentification;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Web.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly IAuthService _authService;

    public UserController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResultDto>> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);
        return !result.Success ? BadRequest(result) : Ok(result);
    }

    [HttpPost("access")]
    [Authorize]
    public async Task<ActionResult<bool>> CheckPageAccess([FromBody] AccessRequestDto request)
    {
        var hasAccess = await _authService.CheckPageAccessAsync(request.TargetPage, request.TargetId, request.TargetKey);
        return Ok(hasAccess);
    }

    [HttpGet("validate")]
    [Authorize]
    public async Task<ActionResult<bool>> ValidateToken()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
        var isValid = await _authService.ValidateTokenAsync(token);
        return Ok(isValid);
    }

    [HttpGet]
    [RequirePermission(Permissions.ManageUsers)]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        var users = await _authService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [RequirePermission(Permissions.ViewUsers)]
    public async Task<ActionResult<UserDto>> GetUserById(string id)
    {
        var user = await _authService.GetUserByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    [RequirePermission(Permissions.ManageUsers)]
    public async Task<ActionResult<AddUserResultDto>> AddUser([FromBody] AddUserRequestDto request)
    {
        var result = await _authService.AddUserAsync(request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut]
    [RequirePermission(Permissions.ManageUsers)]
    public async Task<ActionResult<bool>> UpdateUser([FromBody] UpdateUserRequestDto request)
    {
        var result = await _authService.UpdateUserAsync(request.User, request.Password, request.Roles);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [RequirePermission(Permissions.ManageUsers)]
    public async Task<ActionResult<bool>> DeleteUser(string id)
    {
        var result = await _authService.DeleteUserAsync(id);
        return Ok(result);
    }

    [HttpGet("{id}/roles")]
    [RequirePermission(Permissions.ViewUsers)]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetUserRoles(string id)
    {
        var roles = await _authService.GetUserRolesWithDetailsAsync(id);
        return Ok(roles);
    }

    [HttpPost("{id}/roles")]
    [RequirePermission(Permissions.ManageUsers)]
    public async Task<ActionResult<bool>> AddUserToRole(string id, [FromQuery] string roleName)
    {
        var result = await _authService.AddUserToRoleAsync(id, roleName);
        return Ok(result);
    }

    [HttpDelete("{id}/roles")]
    [RequirePermission(Permissions.ManageUsers)]
    public async Task<ActionResult<bool>> RemoveUserFromRole(string id, [FromQuery] string roleName)
    {
        var result = await _authService.RemoveUserFromRoleAsync(id, roleName);
        return Ok(result);
    }

    [NonAction]
    public void Dispose()
    {
        // Dispose implementation not needed for this controller
    }
}
