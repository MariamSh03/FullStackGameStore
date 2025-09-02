using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using AdminPanel.Bll.Configuration;
using AdminPanel.Bll.Constants;
using AdminPanel.Bll.DTOs.Authentification;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Entity.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AdminPanel.Bll.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ExternalAuthConfig _externalAuthConfig;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UserManager<UserEntity> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        IHttpClientFactory httpClientFactory,
        IOptions<ExternalAuthConfig> externalAuthOptions,
        ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _httpClientFactory = httpClientFactory;
        _externalAuthConfig = externalAuthOptions.Value;
        _logger = logger;
    }

    public async Task<AuthResultDto> LoginAsync(LoginRequestDto request)
    {
        var login = request.Model.Login;
        var password = request.Model.Password;
        var internalAuth = request.Model.InternalAuth;

        _logger.LogInformation("Login attempt for user: {Login}, InternalAuth: {InternalAuth}", login, internalAuth);

        return internalAuth ? await HandleInternalAuthAsync(login, password) : await HandleExternalAuthAsync(login, password);
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return Task.FromResult(false);
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = _configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var key = Encoding.UTF8.GetBytes(secretKey);

        try
        {
            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JwtSettings:Audience"],
                    ClockSkew = TimeSpan.Zero,
                },
                out _);

            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<IEnumerable<ReturnUserDto>> GetAllUsersAsync()
    {
        var users = _userManager.Users.ToList();
        var returnUserDtos = users.Select(u => new ReturnUserDto
        {
            Name = u.UserName,
            Id = u.Id,
        });

        return Task.FromResult(returnUserDtos);
    }

    public async Task<ReturnUserDto> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return user == null ? null : new ReturnUserDto
        {
            Name = user.UserName,
            Id = user.Id,
        };
    }

    public async Task<AddUserResultDto> AddUserAsync(AddUserRequestDto request)
    {
        var userDto = request.User;

        // README requires only name for create
        var desiredUserName = userDto?.Name;

        if (string.IsNullOrWhiteSpace(desiredUserName))
        {
            return new AddUserResultDto
            {
                Success = false,
                Messages = new List<string> { "User name is required." },
            };
        }

        string? desiredEmail = null;

#pragma warning disable CS8601 // Possible null reference assignment.
        var user = new UserEntity
        {
            UserName = desiredUserName,
            Email = desiredEmail,
            IsExternalUser = false,
        };
#pragma warning restore CS8601 // Possible null reference assignment.

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return new AddUserResultDto
            {
                Success = false,
                Messages = result.Errors.Select(e => e.Description).ToList(),
            };
        }

        foreach (var roleId in request.Roles)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role != null)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }
        }

        return new AddUserResultDto { Success = true };
    }

    public async Task<bool> AddUserAsync(UserDto userDto, string password, string roleName)
    {
        var user = new UserEntity
        {
            UserName = userDto.Email,
            Email = userDto.Email,
            IsExternalUser = userDto.IsExternalUser,
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return false;
        }

        var roleResult = await _userManager.AddToRoleAsync(user, roleName);
        return roleResult.Succeeded;
    }

    public async Task<bool> UpdateUserAsync(ReturnUserDto userDto, string password, List<Guid> roleIds)
    {
        var userId = _httpContextAccessor.HttpContext?.Request?.Query["id"].FirstOrDefault() ?? userDto?.Id;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return false;
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(userDto.Name))
        {
            user.UserName = userDto.Name;
        }

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            return false;
        }

        // Update password if provided
        if (!string.IsNullOrWhiteSpace(password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResult = await _userManager.ResetPasswordAsync(user, token, password);
            if (!passwordResult.Succeeded)
            {
                return false;
            }
        }

        // Update roles
        var existingRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, existingRoles);

        foreach (var roleId in roleIds)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role != null)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }
        }

        return true;
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return false;
        }

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user == null ? Enumerable.Empty<string>() : await _userManager.GetRolesAsync(user);
    }

    public async Task<IEnumerable<RoleDto>> GetUserRolesWithDetailsAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Enumerable.Empty<RoleDto>();
        }

        var roleNames = await _userManager.GetRolesAsync(user);
        var allRoles = await _roleManager.Roles.ToListAsync();

        return allRoles.Where(r => roleNames.Contains(r.Name))
                      .Select(r => new RoleDto { Name = r.Name });
    }

    public async Task<bool> AddUserToRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded;
    }

    public async Task<bool> RemoveUserFromRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        var result = await _userManager.RemoveFromRoleAsync(user, roleName);
        return result.Succeeded;
    }

    public async Task<IEnumerable<ReturnRoleDto>> GetAllRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return roles.Select(r => new ReturnRoleDto { Id = r.Id, Name = r.Name });
    }

    public async Task<ReturnRoleDto> GetRoleByIdAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        return role == null ? null : new ReturnRoleDto { Id = id, Name = role.Name };
    }

    public async Task DeleteRoleByIdAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role != null)
        {
            await _roleManager.DeleteAsync(role);
        }
    }

    public async Task<IEnumerable<string>> GetAllPermissionsAsync()
    {
        return await Task.FromResult(Permissions.AllPermissions);
    }

    public async Task<IEnumerable<string>> GetRolePermissionsAsync(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null)
        {
            return Enumerable.Empty<string>();
        }

        var claims = await _roleManager.GetClaimsAsync(role);
        return claims.Where(c => c.Type.StartsWith("permission", StringComparison.Ordinal)).Select(c => c.Value).ToList();
    }

    public async Task<bool> HasAccess(AccessRequestDto dto)
    {
        return await CheckPageAccessAsync(dto.TargetPage, dto.TargetId, dto.TargetKey);
    }

    public async Task<RoleDto> AddRoleAsync(AddRoleRequestDto dto)
    {
        var role = new IdentityRole { Name = dto.Role.Name };
        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Failed to create role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        // Add permissions as claims to the role
        if (dto.Permissions != null && dto.Permissions.Any())
        {
            foreach (var permission in dto.Permissions)
            {
                var claim = new Claim("permission", permission);
                await _roleManager.AddClaimAsync(role, claim);
            }
        }

        return new RoleDto { Name = role.Name };
    }

    public async Task<RoleDto> UpdateRoleAsync(UpdateRoleRequestDto dto)
    {
        var role = await _roleManager.FindByIdAsync(dto.Id) ?? throw new KeyNotFoundException("Role not found.");
        role.Name = dto.Role.Name;

        var result = await _roleManager.UpdateAsync(role);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Failed to update role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        // Update permissions: remove existing permission claims and add new ones
        var existingClaims = await _roleManager.GetClaimsAsync(role);
        var permissionClaims = existingClaims.Where(c => c.Type == "permission").ToList();

        foreach (var claim in permissionClaims)
        {
            await _roleManager.RemoveClaimAsync(role, claim);
        }

        // Add new permissions
        if (dto.Permissions != null && dto.Permissions.Any())
        {
            foreach (var permission in dto.Permissions)
            {
                var claim = new Claim("permission", permission);
                await _roleManager.AddClaimAsync(role, claim);
            }
        }

        return new RoleDto { Name = role.Name };
    }

    public async Task<bool> CheckPageAccessAsync(string targetPage, Guid? targetId = null, string? targetKey = null)
    {
        Console.WriteLine($"CheckPageAccessAsync called for page: {targetPage}");

        var userId = GetCurrentUserId();
        Console.WriteLine($"Current User ID: {userId ?? "NULL"}");

        if (userId == null)
        {
            Console.WriteLine("User ID is null - returning false");
            return false;
        }

        var user = await _userManager.FindByIdAsync(userId);
        Console.WriteLine($"User found: {user?.Email ?? "NULL"}");

        if (user == null)
        {
            Console.WriteLine("User not found in database - returning false");
            return false;
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        if (!userRoles.Any())
        {
            return false;
        }

        var allPermissions = new HashSet<string>();
        foreach (var roleName in userRoles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var rolePermissions = await GetRolePermissionsAsync(role.Id);
                foreach (var permission in rolePermissions)
                {
                    allPermissions.Add(permission);
                }
            }
        }

        // Map page names to required permissions
        var requiredPermission = targetPage switch
        {
            "Games" => Permissions.ViewGame,
            "Genres" => Permissions.ViewGenre,
            "Platforms" => Permissions.ViewPlatform,
            "Publishers" => Permissions.ViewPublisher,
            "Orders" => Permissions.ViewOrders,
            "Users" => Permissions.ViewUsers,
            "Roles" => Permissions.ViewRoles,
            "History" => Permissions.ViewOrderHistory,
            "DeleteGame" => Permissions.DeleteGame,
            _ => targetPage,
        };

        Console.WriteLine($"Required permission for '{targetPage}': {requiredPermission}");
        Console.WriteLine($"User has permissions: {string.Join(", ", allPermissions)}");
        var hasPermission = allPermissions.Contains(requiredPermission);
        Console.WriteLine($"Access granted: {hasPermission}");

        return hasPermission;
    }

    private string GenerateJwtToken(UserEntity user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };
        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
        }

        claims.Add(new Claim(ClaimTypes.Name, user.UserName));

        var roles = _userManager.GetRolesAsync(user).Result;
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // Add permission claims based on user roles
        foreach (var roleName in roles)
        {
            var role = _roleManager.FindByNameAsync(roleName).Result;
            if (role != null)
            {
                var roleClaims = _roleManager.GetClaimsAsync(role).Result;
                var permissionClaims = roleClaims.Where(c => c.Type == "permission");
                claims.AddRange(permissionClaims);
            }
        }

        var secretKey = _configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiryInMinutes"]));

        var token = new JwtSecurityToken(
            _configuration["JwtSettings:Issuer"],
            _configuration["JwtSettings:Audience"],
            claims,
            expires: expires,
            signingCredentials: creds);

        return "Bearer " + new JwtSecurityTokenHandler().WriteToken(token);
    }

    private Task<AuthResultDto> GenerateJwtToken(UserEntity user, bool isExternalUser)
    {
        var token = GenerateJwtToken(user);
        return Task.FromResult(new AuthResultDto
        {
            Success = true,
            Token = token,
            UserId = user.Id.ToString(),
            Email = user.Email ?? user.UserName,
            IsExternalUser = isExternalUser,
        });
    }

    private async Task<AuthResultDto> HandleInternalAuthAsync(string login, string password)
    {
        try
        {
            // Try email first, then username to support email-less accounts
            var user = await _userManager.FindByEmailAsync(login) ?? await _userManager.FindByNameAsync(login);

            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                _logger.LogWarning("Internal authentication failed for user: {Login}", login);
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            _logger.LogInformation("Internal authentication successful for user: {UserId}", user.Id);
            return await GenerateJwtToken(user, isExternalUser: false);
        }
        catch (Exception ex) when (ex is not UnauthorizedAccessException)
        {
            _logger.LogError(ex, "Error during internal authentication for user: {Login}", login);
            throw new UnauthorizedAccessException("Authentication failed");
        }
    }

    private async Task<AuthResultDto> HandleExternalAuthAsync(string login, string password)
    {
        try
        {
            if (string.IsNullOrEmpty(_externalAuthConfig.BaseUrl))
            {
                _logger.LogError("External auth configuration is missing BaseUrl");
                throw new InvalidOperationException("External authentication is not configured");
            }

            using var httpClient = _httpClientFactory.CreateClient("ExternalAuthClient");

            var authRequest = new
            {
                email = login,
                password = password,
            };

            _logger.LogInformation(
                "Attempting external authentication for user: {Login} at {Url}",
                login,
                _externalAuthConfig.GetLoginUrl());

            var response = await httpClient.PostAsJsonAsync(_externalAuthConfig.LoginEndpoint, authRequest);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning(
                    "External auth failed for user: {Login}. Status: {StatusCode}, Response: {Response}",
                    login,
                    response.StatusCode,
                    errorContent);
                throw new UnauthorizedAccessException("External authentication failed");
            }

            _logger.LogInformation("External authentication successful for user: {Login}", login);

            // Find the corresponding internal user
            var internalUser = await _userManager.FindByEmailAsync(login) ?? await _userManager.FindByNameAsync(login);

            if (internalUser == null)
            {
                _logger.LogWarning("User {Login} authenticated externally but not found in internal system", login);
                throw new UnauthorizedAccessException("User not registered internally");
            }

            return await GenerateJwtToken(internalUser, isExternalUser: true);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error during external authentication for user: {Login}", login);
            throw new UnauthorizedAccessException("External authentication service unavailable");
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout during external authentication for user: {Login}", login);
            throw new UnauthorizedAccessException("External authentication timeout");
        }
        catch (Exception ex) when (ex is not (UnauthorizedAccessException or InvalidOperationException))
        {
            _logger.LogError(ex, "Unexpected error during external authentication for user: {Login}", login);
            throw new UnauthorizedAccessException("External authentication failed");
        }
    }

    private string? GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim?.Value;
    }
}