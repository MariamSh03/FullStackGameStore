using AdminPanel.Bll.DTOs.Authentification;

namespace AdminPanel.Bll.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto> LoginAsync(LoginRequestDto request);

    Task<bool> ValidateTokenAsync(string token);

    Task<IEnumerable<ReturnUserDto>> GetAllUsersAsync();

    Task<ReturnUserDto> GetUserByIdAsync(string id);

    Task<AddUserResultDto> AddUserAsync(AddUserRequestDto request);

    Task<bool> AddUserAsync(UserDto userDto, string password, string roleName);

    Task<bool> UpdateUserAsync(ReturnUserDto userDto, string password, List<Guid> roleIds);

    Task<bool> DeleteUserAsync(string id);

    Task<IEnumerable<string>> GetUserRolesAsync(string userId);

    Task<IEnumerable<RoleDto>> GetUserRolesWithDetailsAsync(string userId);

    Task<bool> AddUserToRoleAsync(string userId, string roleName);

    Task<bool> RemoveUserFromRoleAsync(string userId, string roleName);

    // Role Management Methods
    Task<IEnumerable<ReturnRoleDto>> GetAllRolesAsync();

    Task<ReturnRoleDto> GetRoleByIdAsync(string id);

    Task DeleteRoleByIdAsync(string id);

    Task<IEnumerable<string>> GetAllPermissionsAsync();

    Task<IEnumerable<string>> GetRolePermissionsAsync(string id);

    Task<RoleDto> AddRoleAsync(AddRoleRequestDto dto);

    Task<RoleDto> UpdateRoleAsync(UpdateRoleRequestDto dto);

    // Updated to support both Guid and string keys
    Task<bool> CheckPageAccessAsync(string targetPage, Guid? targetId = null, string? targetKey = null);
}
