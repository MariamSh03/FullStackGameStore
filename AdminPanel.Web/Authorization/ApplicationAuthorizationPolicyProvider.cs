using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace AdminPanel.Web.Authorization;

public class ApplicationAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider, IAuthorizationPolicyProvider
{
    public ApplicationAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options)
    {
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);
        if (policy != null)
        {
            return policy;
        }

        // Handle custom policy names here
        if (policyName.StartsWith("RequirePermission:", StringComparison.OrdinalIgnoreCase))
        {
#pragma warning disable IDE0057 // Use range operator
            var permission = policyName.Substring("RequirePermission:".Length);
#pragma warning restore IDE0057 // Use range operator
            return new AuthorizationPolicyBuilder()
                .RequireClaim("permission", permission)
                .Build();
        }

        return null;
    }
}