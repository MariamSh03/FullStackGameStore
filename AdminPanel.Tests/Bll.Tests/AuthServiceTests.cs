using System.Security.Claims;
using AdminPanel.Bll.Configuration;
using AdminPanel.Bll.Constants;
using AdminPanel.Bll.DTOs.Authentification;
using AdminPanel.Bll.Services;
using AdminPanel.Entity.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;

namespace AdminPanel.Tests.Bll.Tests;

public class AuthServiceTests
{
    private readonly Mock<UserManager<UserEntity>> _mockUserManager;
    private readonly Mock<RoleManager<IdentityRole>> _mockRoleManager;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<IOptions<ExternalAuthConfig>> _mockExternalAuthOptions;
    private readonly Mock<ILogger<AuthService>> _mockLogger;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockUserManager = CreateMockUserManager();
        _mockRoleManager = CreateMockRoleManager();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockExternalAuthOptions = new Mock<IOptions<ExternalAuthConfig>>();
        _mockLogger = new Mock<ILogger<AuthService>>();

        // Setup default configuration values
        SetupConfiguration();
        SetupExternalAuthConfig();

        _authService = new AuthService(
            _mockUserManager.Object,
            _mockRoleManager.Object,
            _mockConfiguration.Object,
            _mockHttpContextAccessor.Object,
            _mockHttpClientFactory.Object,
            _mockExternalAuthOptions.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task LoginAsync_WithValidInternalCredentials_ReturnsSuccessResult()
    {
        // Arrange
        var user = new UserEntity
        {
            Id = "user-123",
            Email = "test@example.com",
            UserName = "test@example.com",
        };

        var loginRequest = new LoginRequestDto
        {
            Model = new LoginModel
            {
                Login = "test@example.com",
                Password = "ValidPassword123!",
                InternalAuth = true,
            },
        };

        _mockUserManager.Setup(um => um.FindByEmailAsync("test@example.com"))
                       .ReturnsAsync(user);
        _mockUserManager.Setup(um => um.CheckPasswordAsync(user, "ValidPassword123!"))
                       .ReturnsAsync(true);
        _mockUserManager.Setup(um => um.GetRolesAsync(user))
                       .ReturnsAsync(new List<string> { "User" });

        // Act
        var result = await _authService.LoginAsync(loginRequest);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Token);
        Assert.Equal("user-123", result.UserId);
        Assert.Equal("test@example.com", result.Email);
        Assert.False(result.IsExternalUser);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidInternalCredentials_ThrowsUnauthorizedException()
    {
        // Arrange
        var loginRequest = new LoginRequestDto
        {
            Model = new LoginModel
            {
                Login = "test@example.com",
                Password = "WrongPassword",
                InternalAuth = true,
            },
        };

        _mockUserManager.Setup(um => um.FindByEmailAsync("test@example.com"))
                       .ReturnsAsync((UserEntity)null!);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(loginRequest));
    }

    [Fact]
    public async Task LoginAsync_WithValidPasswordButUserNotFound_ThrowsUnauthorizedException()
    {
        // Arrange
        var loginRequest = new LoginRequestDto
        {
            Model = new LoginModel
            {
                Login = "nonexistent@example.com",
                Password = "ValidPassword123!",
                InternalAuth = true,
            },
        };

        _mockUserManager.Setup(um => um.FindByEmailAsync("nonexistent@example.com"))
                       .ReturnsAsync((UserEntity)null!);
        _mockUserManager.Setup(um => um.FindByNameAsync("nonexistent@example.com"))
                       .ReturnsAsync((UserEntity)null!);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(loginRequest));
    }

    [Fact]
    public async Task ValidateTokenAsync_WithEmptyToken_ReturnsFalse()
    {
        // Act
        var result = await _authService.ValidateTokenAsync(string.Empty);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ValidateTokenAsync_WithNullToken_ReturnsFalse()
    {
        // Act
        var result = await _authService.ValidateTokenAsync(null);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ValidateTokenAsync_WithInvalidToken_ReturnsFalse()
    {
        // Act
        var result = await _authService.ValidateTokenAsync("invalid.jwt.token");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetAllUsersAsync_WhenUsersExist_ReturnsUserDtos()
    {
        // Arrange
        var users = new List<UserEntity>
        {
            new() { Id = "user1", UserName = "User One" },
            new() { Id = "user2", UserName = "User Two" },
        };

        var mockQueryable = users.AsQueryable().BuildMock();
        _mockUserManager.Setup(um => um.Users).Returns(mockQueryable);

        // Act
        var result = await _authService.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, u => u.Name == "User One");
        Assert.Contains(result, u => u.Name == "User Two");
    }

    [Fact]
    public async Task GetAllUsersAsync_WhenNoUsers_ReturnsEmptyCollection()
    {
        // Arrange
        var users = new List<UserEntity>();
        var mockQueryable = users.AsQueryable().BuildMock();
        _mockUserManager.Setup(um => um.Users).Returns(mockQueryable);

        // Act
        var result = await _authService.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserExists_ReturnsUserDto()
    {
        // Arrange
        var userId = "user-123";
        var user = new UserEntity { Id = userId, UserName = "Test User" };

        _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var result = await _authService.GetUserByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal("Test User", result.Name);
    }

    [Fact]
    public async Task GetUserByIdAsync_WhenUserDoesNotExist_ReturnsNull()
    {
        // Arrange
        var userId = "non-existent";
        _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync((UserEntity)null!);

        // Act
        var result = await _authService.GetUserByIdAsync(userId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddUserAsync_WithValidRequest_ReturnsSuccessResult()
    {
        // Arrange
        var request = new AddUserRequestDto
        {
            User = new CreateUserDto { Name = "New User" },
            Password = "ValidPassword123!",
            Roles = new List<Guid> { Guid.NewGuid() },
        };

        var createdUser = new UserEntity { Id = "new-user-123", UserName = "New User" };

        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<UserEntity>(), request.Password))
                       .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(um => um.FindByNameAsync("New User"))
                       .ReturnsAsync(createdUser);

        var role = new IdentityRole { Id = request.Roles[0].ToString(), Name = "TestRole" };
        _mockRoleManager.Setup(rm => rm.FindByIdAsync(request.Roles[0].ToString()))
                       .ReturnsAsync(role);
        _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<UserEntity>(), "TestRole"))
                       .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authService.AddUserAsync(request);

        // Assert
        Assert.True(result.Success);
        Assert.Empty(result.Messages);
    }

    [Fact]
    public async Task AddUserAsync_WithEmptyUserName_ReturnsFailureResult()
    {
        // Arrange
        var request = new AddUserRequestDto
        {
            User = new CreateUserDto { Name = string.Empty },
            Password = "ValidPassword123!",
            Roles = new List<Guid>(),
        };

        // Act
        var result = await _authService.AddUserAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("User name is required.", result.Messages);
    }

    [Fact]
    public async Task AddUserAsync_WithNullUser_ReturnsFailureResult()
    {
        // Arrange
        var request = new AddUserRequestDto
        {
            User = null,
            Password = "ValidPassword123!",
            Roles = new List<Guid>(),
        };

        // Act
        var result = await _authService.AddUserAsync(request);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("User name is required.", result.Messages);
    }

    [Fact]
    public async Task UpdateUserAsync_WithValidUser_ReturnsTrue()
    {
        // Arrange
        var userDto = new ReturnUserDto { Id = "user-123", Name = "Updated User" };
        var password = "NewPassword123!";
        var roleIds = new List<Guid> { Guid.NewGuid() };

        var user = new UserEntity { Id = "user-123", UserName = "Original User" };
        var role = new IdentityRole { Id = roleIds[0].ToString(), Name = "TestRole" };

        _mockUserManager.Setup(um => um.FindByIdAsync("user-123")).ReturnsAsync(user);
        _mockUserManager.Setup(um => um.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(um => um.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("reset-token");
        _mockUserManager.Setup(um => um.ResetPasswordAsync(user, "reset-token", password))
                       .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string>());
        _mockUserManager.Setup(um => um.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()))
                       .ReturnsAsync(IdentityResult.Success);
        _mockRoleManager.Setup(rm => rm.FindByIdAsync(roleIds[0].ToString())).ReturnsAsync(role);
        _mockUserManager.Setup(um => um.AddToRoleAsync(user, "TestRole"))
                       .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authService.UpdateUserAsync(userDto, password, roleIds);

        // Assert
        Assert.True(result);
        Assert.Equal("Updated User", user.UserName);
    }

    [Fact]
    public async Task UpdateUserAsync_WithNonExistentUser_ReturnsFalse()
    {
        // Arrange
        var userDto = new ReturnUserDto { Id = "non-existent", Name = "Updated User" };
        var password = "NewPassword123!";
        var roleIds = new List<Guid>();

        _mockUserManager.Setup(um => um.FindByIdAsync("non-existent")).ReturnsAsync((UserEntity)null!);

        // Act
        var result = await _authService.UpdateUserAsync(userDto, password, roleIds);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteUserAsync_WithExistingUser_ReturnsTrue()
    {
        // Arrange
        var userId = "user-123";
        var user = new UserEntity { Id = userId, UserName = "Test User" };

        _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
        _mockUserManager.Setup(um => um.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authService.DeleteUserAsync(userId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteUserAsync_WithNonExistentUser_ReturnsFalse()
    {
        // Arrange
        var userId = "non-existent";
        _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync((UserEntity)null!);

        // Act
        var result = await _authService.DeleteUserAsync(userId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetUserRolesAsync_WithExistingUser_ReturnsRoles()
    {
        // Arrange
        var userId = "user-123";
        var user = new UserEntity { Id = userId, UserName = "Test User" };
        var roles = new List<string> { "Admin", "User" };

        _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
        _mockUserManager.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(roles);

        // Act
        var result = await _authService.GetUserRolesAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains("Admin", result);
        Assert.Contains("User", result);
    }

    [Fact]
    public async Task GetUserRolesAsync_WithNonExistentUser_ReturnsEmptyCollection()
    {
        // Arrange
        var userId = "non-existent";
        _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync((UserEntity)null!);

        // Act
        var result = await _authService.GetUserRolesAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddUserToRoleAsync_WithValidUserAndRole_ReturnsTrue()
    {
        // Arrange
        var userId = "user-123";
        var roleName = "Manager";
        var user = new UserEntity { Id = userId, UserName = "Test User" };

        _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
        _mockUserManager.Setup(um => um.AddToRoleAsync(user, roleName)).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authService.AddUserToRoleAsync(userId, roleName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AddUserToRoleAsync_WithNonExistentUser_ReturnsFalse()
    {
        // Arrange
        var userId = "non-existent";
        var roleName = "Manager";

        _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync((UserEntity)null!);

        // Act
        var result = await _authService.AddUserToRoleAsync(userId, roleName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task RemoveUserFromRoleAsync_WithValidUserAndRole_ReturnsTrue()
    {
        // Arrange
        var userId = "user-123";
        var roleName = "Manager";
        var user = new UserEntity { Id = userId, UserName = "Test User" };

        _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
        _mockUserManager.Setup(um => um.RemoveFromRoleAsync(user, roleName)).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authService.RemoveUserFromRoleAsync(userId, roleName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task RemoveUserFromRoleAsync_WithNonExistentUser_ReturnsFalse()
    {
        // Arrange
        var userId = "non-existent";
        var roleName = "Manager";

        _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync((UserEntity)null!);

        // Act
        var result = await _authService.RemoveUserFromRoleAsync(userId, roleName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetAllRolesAsync_WhenRolesExist_ReturnsRoleDtos()
    {
        // Arrange
        var roles = new List<IdentityRole>
        {
            new() { Id = "role1", Name = "Admin" },
            new() { Id = "role2", Name = "User" },
        };

        var mockQueryable = roles.AsQueryable().BuildMock();
        _mockRoleManager.Setup(rm => rm.Roles).Returns(mockQueryable);

        // Act
        var result = await _authService.GetAllRolesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.Name == "Admin");
        Assert.Contains(result, r => r.Name == "User");
    }

    [Fact]
    public async Task GetRoleByIdAsync_WithExistingRole_ReturnsRoleDto()
    {
        // Arrange
        var roleId = "role-123";
        var role = new IdentityRole { Id = roleId, Name = "Admin" };

        _mockRoleManager.Setup(rm => rm.FindByIdAsync(roleId)).ReturnsAsync(role);

        // Act
        var result = await _authService.GetRoleByIdAsync(roleId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(roleId, result.Id);
        Assert.Equal("Admin", result.Name);
    }

    [Fact]
    public async Task GetRoleByIdAsync_WithNonExistentRole_ReturnsNull()
    {
        // Arrange
        var roleId = "non-existent";
        _mockRoleManager.Setup(rm => rm.FindByIdAsync(roleId)).ReturnsAsync((IdentityRole)null!);

        // Act
        var result = await _authService.GetRoleByIdAsync(roleId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteRoleByIdAsync_WithExistingRole_DeletesRole()
    {
        // Arrange
        var roleId = "role-123";
        var role = new IdentityRole { Id = roleId, Name = "TestRole" };

        _mockRoleManager.Setup(rm => rm.FindByIdAsync(roleId)).ReturnsAsync(role);
        _mockRoleManager.Setup(rm => rm.DeleteAsync(role)).ReturnsAsync(IdentityResult.Success);

        // Act
        await _authService.DeleteRoleByIdAsync(roleId);

        // Assert
        _mockRoleManager.Verify(rm => rm.DeleteAsync(role), Times.Once);
    }

    [Fact]
    public async Task DeleteRoleByIdAsync_WithNonExistentRole_DoesNotThrow()
    {
        // Arrange
        var roleId = "non-existent";
        _mockRoleManager.Setup(rm => rm.FindByIdAsync(roleId)).ReturnsAsync((IdentityRole)null!);

        // Act & Assert
        await _authService.DeleteRoleByIdAsync(roleId);
        _mockRoleManager.Verify(rm => rm.DeleteAsync(It.IsAny<IdentityRole>()), Times.Never);
    }

    [Fact]
    public async Task GetAllPermissionsAsync_ReturnsAllPermissions()
    {
        // Act
        var result = await _authService.GetAllPermissionsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);

        // Verify some expected permissions exist
        Assert.Contains(Permissions.ViewGame, result);
        Assert.Contains(Permissions.ManageUsers, result);
        Assert.Contains(Permissions.ViewRoles, result);
    }

    [Fact]
    public async Task AddRoleAsync_WithValidRequest_ReturnsRoleDto()
    {
        // Arrange
        var request = new AddRoleRequestDto
        {
            Role = new RoleDto { Name = "CustomRole" },
            Permissions = new List<string> { "ViewGames", "ManageGames" },
        };

        var createdRole = new IdentityRole { Id = "new-role-id", Name = "CustomRole" };
        _mockRoleManager.Setup(rm => rm.CreateAsync(It.IsAny<IdentityRole>()))
                       .ReturnsAsync(IdentityResult.Success)
                       .Callback<IdentityRole>(r => r.Id = "new-role-id");
        _mockRoleManager.Setup(rm => rm.AddClaimAsync(It.IsAny<IdentityRole>(), It.IsAny<Claim>()))
                       .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authService.AddRoleAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("CustomRole", result.Name);
        _mockRoleManager.Verify(rm => rm.AddClaimAsync(It.IsAny<IdentityRole>(), It.IsAny<Claim>()), Times.Exactly(2));
    }

    [Fact]
    public async Task AddRoleAsync_WhenRoleCreationFails_ThrowsException()
    {
        // Arrange
        var request = new AddRoleRequestDto
        {
            Role = new RoleDto { Name = "DuplicateRole" },
            Permissions = new List<string>(),
        };

        var errors = new List<IdentityError> { new() { Description = "Role already exists" } };
        _mockRoleManager.Setup(rm => rm.CreateAsync(It.IsAny<IdentityRole>()))
                       .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.AddRoleAsync(request));
        Assert.Contains("Failed to create role", exception.Message);
    }

    [Fact]
    public async Task UpdateRoleAsync_WithValidRequest_ReturnsUpdatedRoleDto()
    {
        // Arrange
        var request = new UpdateRoleRequestDto
        {
            Id = "role-123",
            Role = new RoleDto { Name = "UpdatedRole" },
            Permissions = new List<string> { "ViewGames" },
        };

        var role = new IdentityRole { Id = "role-123", Name = "OriginalRole" };
        var existingClaims = new List<Claim>
        {
            new("permission", "OldPermission"),
            new("other", "OtherClaim"),
        };

        _mockRoleManager.Setup(rm => rm.FindByIdAsync("role-123")).ReturnsAsync(role);
        _mockRoleManager.Setup(rm => rm.UpdateAsync(role)).ReturnsAsync(IdentityResult.Success);
        _mockRoleManager.Setup(rm => rm.GetClaimsAsync(role)).ReturnsAsync(existingClaims);
        _mockRoleManager.Setup(rm => rm.RemoveClaimAsync(role, It.IsAny<Claim>()))
                       .ReturnsAsync(IdentityResult.Success);
        _mockRoleManager.Setup(rm => rm.AddClaimAsync(role, It.IsAny<Claim>()))
                       .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authService.UpdateRoleAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("UpdatedRole", result.Name);
        Assert.Equal("UpdatedRole", role.Name);
        _mockRoleManager.Verify(rm => rm.RemoveClaimAsync(role, It.Is<Claim>(c => c.Type == "permission")), Times.Once);
        _mockRoleManager.Verify(rm => rm.AddClaimAsync(role, It.IsAny<Claim>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRoleAsync_WithNonExistentRole_ThrowsKeyNotFoundException()
    {
        // Arrange
        var request = new UpdateRoleRequestDto
        {
            Id = "non-existent",
            Role = new RoleDto { Name = "UpdatedRole" },
            Permissions = new List<string>(),
        };

        _mockRoleManager.Setup(rm => rm.FindByIdAsync("non-existent")).ReturnsAsync((IdentityRole)null!);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _authService.UpdateRoleAsync(request));
    }

    [Fact]
    public async Task CheckPageAccessAsync_WithNoCurrentUser_ReturnsFalse()
    {
        // Arrange
        SetupHttpContext(null); // No user ID in context

        // Act
        var result = await _authService.CheckPageAccessAsync("Games");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetUserRolesWithDetailsAsync_WithValidUser_ReturnsRoleDetails()
    {
        // Arrange
        var userId = "user-123";
        var user = new UserEntity { Id = userId, UserName = "Test User" };
        var userRoles = new List<string> { "Admin", "User" };
        var allRoles = new List<IdentityRole>
        {
            new() { Id = "role1", Name = "Admin" },
            new() { Id = "role2", Name = "User" },
            new() { Id = "role3", Name = "Manager" }, // This role should not be included
        };

        _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
        _mockUserManager.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(userRoles);

        var mockQueryable = allRoles.AsQueryable().BuildMock();
        _mockRoleManager.Setup(rm => rm.Roles).Returns(mockQueryable);

        // Act
        var result = await _authService.GetUserRolesWithDetailsAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.Name == "Admin");
        Assert.Contains(result, r => r.Name == "User");
        Assert.DoesNotContain(result, r => r.Name == "Manager");
    }

    [Fact]
    public async Task AddUserAsync_WithSimpleUserDto_ReturnsTrue()
    {
        // Arrange
        var userDto = new UserDto
        {
            Email = "test@example.com",
            UserName = "testuser",
            IsExternalUser = false,
        };
        var password = "ValidPassword123!";
        var roleName = "User";

        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<UserEntity>(), password))
                       .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<UserEntity>(), roleName))
                       .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authService.AddUserAsync(userDto, password, roleName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AddUserAsync_WhenUserCreationFails_ReturnsFalse()
    {
        // Arrange
        var userDto = new UserDto
        {
            Email = "test@example.com",
            UserName = "testuser",
            IsExternalUser = false,
        };
        var password = "WeakPassword";
        var roleName = "User";

        var errors = new List<IdentityError> { new() { Description = "Password too weak" } };
        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<UserEntity>(), password))
                       .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

        // Act
        var result = await _authService.AddUserAsync(userDto, password, roleName);

        // Assert
        Assert.False(result);
    }

    private void SetupHttpContext(string userId)
    {
        var context = new DefaultHttpContext();
        if (userId != null)
        {
            var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userId) };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);
            context.User = principal;
        }

        _mockHttpContextAccessor.Setup(hca => hca.HttpContext).Returns(context);
    }

    private void SetupConfiguration()
    {
        _mockConfiguration.Setup(c => c["JwtSettings:SecretKey"])
            .Returns("ThisIsMySecretKeyForTestingPurposesOnly12345");
        _mockConfiguration.Setup(c => c["JwtSettings:Issuer"])
            .Returns("TestIssuer");
        _mockConfiguration.Setup(c => c["JwtSettings:Audience"])
            .Returns("TestAudience");
        _mockConfiguration.Setup(c => c["JwtSettings:ExpiryInMinutes"])
            .Returns("60");
    }

    private void SetupExternalAuthConfig()
    {
        var externalAuthConfig = new ExternalAuthConfig
        {
            BaseUrl = "https://test-external-auth.com",
            LoginEndpoint = "/login",
            ValidateEndpoint = "/validate",
        };
        _mockExternalAuthOptions.Setup(o => o.Value).Returns(externalAuthConfig);
    }

    private static Mock<UserManager<UserEntity>> CreateMockUserManager()
    {
        var store = new Mock<IUserStore<UserEntity>>();
        return new Mock<UserManager<UserEntity>>(
            store.Object, null, null, null, null, null, null, null, null);
    }

    private static Mock<RoleManager<IdentityRole>> CreateMockRoleManager()
    {
        var store = new Mock<IRoleStore<IdentityRole>>();
        return new Mock<RoleManager<IdentityRole>>(
            store.Object, null, null, null, null);
    }
}