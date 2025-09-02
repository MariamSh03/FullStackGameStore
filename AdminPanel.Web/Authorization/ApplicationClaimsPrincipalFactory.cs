using System.Security.Claims;
using AdminPanel.Entity.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AdminPanel.Web.Authorization;

public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<UserEntity>
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationClaimsPrincipalFactory(
        UserManager<UserEntity> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, optionsAccessor)
    {
        _roleManager = roleManager;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(UserEntity user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        // Add custom claims here
        if (user.IsExternalUser)
        {
            identity.AddClaim(new Claim("IsExternalUser", "true"));
        }

        // Add permission claims based on user roles
        var userRoles = await UserManager.GetRolesAsync(user);
        foreach (var roleName in userRoles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                var permissionClaims = roleClaims.Where(c => c.Type == "permission");
                foreach (var claim in permissionClaims)
                {
                    identity.AddClaim(claim);
                }
            }
        }

        return identity;
    }
}