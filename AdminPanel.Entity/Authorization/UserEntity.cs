using Microsoft.AspNetCore.Identity;

namespace AdminPanel.Entity.Authorization;

public class UserEntity : IdentityUser
{
    public override string UserName { get; set; }

    public override string? Email { get; set; }

    public bool IsExternalUser { get; set; }
}
