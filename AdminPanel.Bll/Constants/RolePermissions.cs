namespace AdminPanel.Bll.Constants;

public static class RolePermissions
{
    // Role hierarchy: Admin => Manager => Moderator => User => Guest
    private static readonly List<string> RoleHierarchy = new() { "Guest", "User", "Moderator", "Manager", "Admin" };

    // Base permissions for each role (without inheritance)
    private static readonly Dictionary<string, List<string>> BaseRolePermissions = new()
    {
        ["Guest"] = new List<string>
        {
            // Read-only access
            Permissions.ViewGame, Permissions.ViewGenre, Permissions.ViewPublisher, Permissions.ViewPlatform,
        },

        ["User"] = new List<string>
        {
            // Can comment on games and view orders
            Permissions.CommentOnGames, Permissions.ViewOrders,
        },

        ["Moderator"] = new List<string>
        {
            // Comment Management & User Banning
            Permissions.ManageComments, Permissions.DeleteComments, Permissions.BanUsers,
        },

        ["Manager"] = new List<string>
        {
            // Business Entity Management
            Permissions.AddGame, Permissions.UpdateGame, Permissions.DeleteGame,
            Permissions.AddGenre, Permissions.UpdateGenre, Permissions.DeleteGenre,
            Permissions.AddPublisher, Permissions.UpdatePublisher, Permissions.DeletePublisher,
            Permissions.AddPlatform, Permissions.UpdatePlatform, Permissions.DeletePlatform,

            // Order Management (can edit orders, view history, ship orders)
            Permissions.EditOrders, Permissions.ViewOrderHistory, Permissions.ShipOrders,
        },

        ["Admin"] = new List<string>
        {
            // User & Role Management (Admin-specific)
            Permissions.ManageUsers, Permissions.ViewUsers,
            Permissions.ManageRoles, Permissions.ViewRoles,

            // Admin-specific Game Management (deleted games)
            Permissions.ViewDeletedGames, Permissions.EditDeletedGames,
        },
    };

    // Computed permissions with inheritance - using lazy initialization
    private static Dictionary<string, List<string>>? _defaultRolePermissions;

    public static Dictionary<string, List<string>> DefaultRolePermissions
    {
        get
        {
            _defaultRolePermissions ??= ComputeRolePermissions();
            return _defaultRolePermissions;
        }
    }

    public static List<string> GetPermissionsForRole(string roleName)
    {
        return DefaultRolePermissions.GetValueOrDefault(roleName, new List<string>());
    }

    public static List<string> GetBasePermissionsForRole(string roleName)
    {
        return BaseRolePermissions.GetValueOrDefault(roleName, new List<string>());
    }

    public static bool IsRoleHigherInHierarchy(string role1, string role2)
    {
        var index1 = RoleHierarchy.IndexOf(role1);
        var index2 = RoleHierarchy.IndexOf(role2);
        return index1 > index2;
    }

    private static Dictionary<string, List<string>> ComputeRolePermissions()
    {
        var result = new Dictionary<string, List<string>>();

        foreach (var roleName in RoleHierarchy)
        {
            var allPermissions = new HashSet<string>();

            // Add permissions from current role and all roles below in hierarchy
            var currentRoleIndex = RoleHierarchy.IndexOf(roleName);
            for (int i = 0; i <= currentRoleIndex; i++)
            {
                var lowerRole = RoleHierarchy[i];
                if (BaseRolePermissions.ContainsKey(lowerRole))
                {
                    foreach (var permission in BaseRolePermissions[lowerRole])
                    {
                        allPermissions.Add(permission);
                    }
                }
            }

            result[roleName] = allPermissions.ToList();
        }

        return result;
    }
}