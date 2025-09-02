using AdminPanel.Bll.DTOs.Authentification;
using AdminPanel.Bll.Interfaces;

namespace AdminPanel.Bll.Services;

public class UserSeeder
{
    private readonly IAuthService _authService;

    public UserSeeder(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task SeedDefaultUsersAsync()
    {
        var existingUsers = await _authService.GetAllUsersAsync();

        async Task CreateUserIfNotExists(string name, string password, string role)
        {
            if (!existingUsers.Any(u => u.Name == name))
            {
                var user = new UserDto
                {
                    UserName = name,
                    IsExternalUser = false,
                };

                var success = await _authService.AddUserAsync(user, password, role);

                Console.WriteLine(success
                    ? $"Default {role} user created successfully! Name: {name}, Password: {password}"
                    : $"Failed to create {role} user");
            }
        }

        await CreateUserIfNotExists("admin@gamestore.com", "Admin123!", "Admin");
        await CreateUserIfNotExists("manager@gamestore.com", "Manager123!", "Manager");
        await CreateUserIfNotExists("moderator@gamestore.com", "Moderator123!", "Moderator");
        await CreateUserIfNotExists("user@gamestore.com", "User123!", "User");
        await CreateUserIfNotExists("guest@gamestore.com", "Guest123!", "Guest");
    }
}