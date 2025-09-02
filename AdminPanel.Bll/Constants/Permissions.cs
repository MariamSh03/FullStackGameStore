namespace AdminPanel.Bll.Constants;

public static class Permissions
{
    // User Management
    public const string ManageUsers = "ManageUsers";
    public const string ViewUsers = "ViewUsers";

    // Role Management
    public const string ManageRoles = "ManageRoles";
    public const string ViewRoles = "ViewRoles";

    // Game Management
    public const string AddGame = "AddGame";
    public const string UpdateGame = "UpdateGame";
    public const string DeleteGame = "DeleteGame";
    public const string ViewGame = "ViewGame";
    public const string ViewDeletedGames = "ViewDeletedGames";
    public const string EditDeletedGames = "EditDeletedGames";

    // Genre Management
    public const string AddGenre = "AddGenre";
    public const string UpdateGenre = "UpdateGenre";
    public const string DeleteGenre = "DeleteGenre";
    public const string ViewGenre = "ViewGenre";

    // Publisher Management
    public const string AddPublisher = "AddPublisher";
    public const string UpdatePublisher = "UpdatePublisher";
    public const string DeletePublisher = "DeletePublisher";
    public const string ViewPublisher = "ViewPublisher";

    // Platform Management
    public const string AddPlatform = "AddPlatform";
    public const string UpdatePlatform = "UpdatePlatform";
    public const string DeletePlatform = "DeletePlatform";
    public const string ViewPlatform = "ViewPlatform";

    // Order Management
    public const string EditOrders = "EditOrders";
    public const string ViewOrderHistory = "ViewOrderHistory";
    public const string ShipOrders = "ShipOrders";
    public const string ViewOrders = "ViewOrders";

    // Comment Management
    public const string ManageComments = "ManageComments";
    public const string DeleteComments = "DeleteComments";
    public const string BanUsers = "BanUsers";
    public const string CommentOnGames = "CommentOnGames";

    // Get all permissions as a list
    public static readonly IReadOnlyList<string> AllPermissions = new List<string>
    {
        ManageUsers, ViewUsers,
        ManageRoles, ViewRoles,
        AddGame, UpdateGame, DeleteGame, ViewGame, ViewDeletedGames, EditDeletedGames,
        AddGenre, UpdateGenre, DeleteGenre, ViewGenre,
        AddPublisher, UpdatePublisher, DeletePublisher, ViewPublisher,
        AddPlatform, UpdatePlatform, DeletePlatform, ViewPlatform,
        EditOrders, ViewOrderHistory, ShipOrders, ViewOrders,
        ManageComments, DeleteComments, BanUsers, CommentOnGames,
    };
}