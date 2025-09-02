using System.Security.Claims;
using AdminPanel.Bll.Constants;
using Microsoft.AspNetCore.Identity;

namespace AdminPanel.Bll.Services;

public class RolePermissionSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolePermissionSeeder(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task SeedRolePermissionsAsync()
    {
        foreach (var rolePermission in RolePermissions.DefaultRolePermissions)
        {
            var roleName = rolePermission.Key;
            var permissions = rolePermission.Value;

            // Create role if it doesn't exist
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                var createResult = await _roleManager.CreateAsync(role);
                if (!createResult.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to create role '{roleName}': {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                }
            }

            // Get existing permission claims for this role
            var existingClaims = await _roleManager.GetClaimsAsync(role);
            var existingPermissions = existingClaims
                .Where(c => c.Type == "permission")
                .Select(c => c.Value)
                .ToHashSet();

            // Add missing permissions
            foreach (var permission in permissions)
            {
                if (!existingPermissions.Contains(permission))
                {
                    var claim = new Claim("permission", permission);
                    await _roleManager.AddClaimAsync(role, claim);
                }
            }
        }
    }
}