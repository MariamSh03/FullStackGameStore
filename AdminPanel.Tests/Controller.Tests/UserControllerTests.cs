using AdminPanel.Bll.DTOs.Authentification;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AdminPanel.Tests.Controller.Tests;

public class UserControllerTests : IDisposable
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _controller = new UserController(_mockAuthService.Object);
    }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
    {
        // UserController doesn't implement IDisposable properly, so we skip disposal
    }
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize

    [Fact]
    public async Task Login_WhenLoginSuccessful_ReturnsOkResult()
    {
        // Arrange
        var loginRequest = new LoginRequestDto
        {
            Model = new LoginModel
            {
                Login = "test@example.com",
                Password = "TestPassword123!",
                InternalAuth = true,
            },
        };

        var authResult = new AuthResultDto
        {
            Success = true,
            Token = "test-jwt-token",
            UserId = "user-123",
            Email = "test@example.com",
            Message = "Login successful",
        };

        _mockAuthService.Setup(s => s.LoginAsync(loginRequest)).ReturnsAsync(authResult);

        // Act
        var result = await _controller.Login(loginRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedResult = Assert.IsType<AuthResultDto>(okResult.Value);
        Assert.True(returnedResult.Success);
        Assert.Equal("test-jwt-token", returnedResult.Token);
        Assert.Equal("user-123", returnedResult.UserId);
    }

    [Fact]
    public async Task Login_WhenLoginFails_ReturnsBadRequest()
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

        var authResult = new AuthResultDto
        {
            Success = false,
            Message = "Invalid credentials",
        };

        _mockAuthService.Setup(s => s.LoginAsync(loginRequest)).ReturnsAsync(authResult);

        // Act
        var result = await _controller.Login(loginRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var returnedResult = Assert.IsType<AuthResultDto>(badRequestResult.Value);
        Assert.False(returnedResult.Success);
        Assert.Equal("Invalid credentials", returnedResult.Message);
    }

    [Fact]
    public async Task CheckPageAccess_WhenUserHasAccess_ReturnsTrue()
    {
        // Arrange
        var accessRequest = new AccessRequestDto
        {
            TargetPage = "users",
            TargetId = Guid.NewGuid(),
            TargetKey = "test-key",
        };

        _mockAuthService.Setup(s => s.CheckPageAccessAsync(accessRequest.TargetPage, accessRequest.TargetId, accessRequest.TargetKey))
                       .ReturnsAsync(true);

        // Act
        var result = await _controller.CheckPageAccess(accessRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var hasAccess = Assert.IsType<bool>(okResult.Value);
        Assert.True(hasAccess);
    }

    [Fact]
    public async Task CheckPageAccess_WhenUserDoesNotHaveAccess_ReturnsFalse()
    {
        // Arrange
        var accessRequest = new AccessRequestDto
        {
            TargetPage = "admin",
            TargetId = Guid.NewGuid(),
            TargetKey = "admin-key",
        };

        _mockAuthService.Setup(s => s.CheckPageAccessAsync(accessRequest.TargetPage, accessRequest.TargetId, accessRequest.TargetKey))
                       .ReturnsAsync(false);

        // Act
        var result = await _controller.CheckPageAccess(accessRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var hasAccess = Assert.IsType<bool>(okResult.Value);
        Assert.False(hasAccess);
    }

    [Fact]
    public async Task ValidateToken_WhenTokenIsValid_ReturnsTrue()
    {
        // Arrange
        var token = "valid-jwt-token";

        // Setup HTTP context with Authorization header
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = $"Bearer {token}";
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = context,
        };

        _mockAuthService.Setup(s => s.ValidateTokenAsync(token)).ReturnsAsync(true);

        // Act
        var result = await _controller.ValidateToken();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var isValid = Assert.IsType<bool>(okResult.Value);
        Assert.True(isValid);
    }

    [Fact]
    public async Task ValidateToken_WhenTokenIsInvalid_ReturnsFalse()
    {
        // Arrange
        var token = "invalid-jwt-token";

        // Setup HTTP context with Authorization header
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = $"Bearer {token}";
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = context,
        };

        _mockAuthService.Setup(s => s.ValidateTokenAsync(token)).ReturnsAsync(false);

        // Act
        var result = await _controller.ValidateToken();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var isValid = Assert.IsType<bool>(okResult.Value);
        Assert.False(isValid);
    }

    [Fact]
    public async Task GetAllUsers_WhenUsersExist_ReturnsOkWithUsers()
    {
        // Arrange
        var users = new List<ReturnUserDto>
        {
            new() { Id = "user1", Name = "User One" },
            new() { Id = "user2", Name = "User Two" },
        };

        _mockAuthService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);

        // Act
        var result = await _controller.GetAllUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUsers = Assert.IsAssignableFrom<IEnumerable<ReturnUserDto>>(okResult.Value);
        Assert.Equal(2, returnedUsers.Count());
    }

    [Fact]
    public async Task GetAllUsers_WhenNoUsersExist_ReturnsEmptyList()
    {
        // Arrange
        var users = new List<ReturnUserDto>();
        _mockAuthService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(users);

        // Act
        var result = await _controller.GetAllUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUsers = Assert.IsAssignableFrom<IEnumerable<ReturnUserDto>>(okResult.Value);
        Assert.Empty(returnedUsers);
    }

    [Fact]
    public async Task GetUserById_WhenUserExists_ReturnsOkWithUser()
    {
        // Arrange
        var userId = "user-123";
        var user = new ReturnUserDto { Id = userId, Name = "Test User" };

        _mockAuthService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var result = await _controller.GetUserById(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUser = Assert.IsType<ReturnUserDto>(okResult.Value);
        Assert.Equal(userId, returnedUser.Id);
        Assert.Equal("Test User", returnedUser.Name);
    }

    [Fact]
    public async Task GetUserById_WhenUserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var userId = "non-existent-user";
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _mockAuthService.Setup(s => s.GetUserByIdAsync(userId)).ReturnsAsync((ReturnUserDto)null);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Act
        var result = await _controller.GetUserById(userId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task AddUser_WhenUserAddedSuccessfully_ReturnsOkResult()
    {
        // Arrange
        var addUserRequest = new AddUserRequestDto
        {
            User = new CreateUserDto { Name = "New User" },
            Password = "NewPassword123!",
            Roles = new List<Guid> { Guid.NewGuid() },
        };

        var addResult = new AddUserResultDto
        {
            Success = true,
            Messages = new List<string> { "User created successfully" },
        };

        _mockAuthService.Setup(s => s.AddUserAsync(addUserRequest)).ReturnsAsync(addResult);

        // Act
        var result = await _controller.AddUser(addUserRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedResult = Assert.IsType<AddUserResultDto>(okResult.Value);
        Assert.True(returnedResult.Success);
    }

    [Fact]
    public async Task AddUser_WhenUserAdditionFails_ReturnsBadRequest()
    {
        // Arrange
        var addUserRequest = new AddUserRequestDto
        {
            User = new CreateUserDto { Name = "Invalid User" },
            Password = "weak",
            Roles = new List<Guid>(),
        };

        var addResult = new AddUserResultDto
        {
            Success = false,
            Messages = new List<string> { "Password is too weak", "At least one role is required" },
        };

        _mockAuthService.Setup(s => s.AddUserAsync(addUserRequest)).ReturnsAsync(addResult);

        // Act
        var result = await _controller.AddUser(addUserRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var returnedResult = Assert.IsType<AddUserResultDto>(badRequestResult.Value);
        Assert.False(returnedResult.Success);
        Assert.Contains("Password is too weak", returnedResult.Messages);
    }

    [Fact]
    public async Task UpdateUser_WhenUpdateSuccessful_ReturnsOkResult()
    {
        // Arrange
        var updateRequest = new UpdateUserRequestDto
        {
            User = new ReturnUserDto { Id = "user-123", Name = "Updated User" },
            Password = "NewPassword123!",
            Roles = new List<Guid> { Guid.NewGuid() },
        };

        _mockAuthService.Setup(s => s.UpdateUserAsync(updateRequest.User, updateRequest.Password, updateRequest.Roles))
                       .ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateUser(updateRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var success = Assert.IsType<bool>(okResult.Value);
        Assert.True(success);
    }

    [Fact]
    public async Task UpdateUser_WhenUpdateFails_ReturnsOkWithFalse()
    {
        // Arrange
        var updateRequest = new UpdateUserRequestDto
        {
            User = new ReturnUserDto { Id = "non-existent", Name = "Updated User" },
            Password = "NewPassword123!",
            Roles = new List<Guid>(),
        };

        _mockAuthService.Setup(s => s.UpdateUserAsync(updateRequest.User, updateRequest.Password, updateRequest.Roles))
                       .ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateUser(updateRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var success = Assert.IsType<bool>(okResult.Value);
        Assert.False(success);
    }

    [Fact]
    public async Task DeleteUser_WhenDeletionSuccessful_ReturnsOkResult()
    {
        // Arrange
        var userId = "user-123";
        _mockAuthService.Setup(s => s.DeleteUserAsync(userId)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var success = Assert.IsType<bool>(okResult.Value);
        Assert.True(success);
    }

    [Fact]
    public async Task DeleteUser_WhenDeletionFails_ReturnsOkWithFalse()
    {
        // Arrange
        var userId = "non-existent-user";
        _mockAuthService.Setup(s => s.DeleteUserAsync(userId)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var success = Assert.IsType<bool>(okResult.Value);
        Assert.False(success);
    }

    [Fact]
    public async Task GetUserRoles_WhenUserHasRoles_ReturnsOkWithRoles()
    {
        // Arrange
        var userId = "user-123";
        var roles = new List<RoleDto>
        {
            new() { Name = "User" },
            new() { Name = "Moderator" },
        };

        _mockAuthService.Setup(s => s.GetUserRolesWithDetailsAsync(userId)).ReturnsAsync(roles);

        // Act
        var result = await _controller.GetUserRoles(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedRoles = Assert.IsAssignableFrom<IEnumerable<RoleDto>>(okResult.Value);
        Assert.Equal(2, returnedRoles.Count());
        Assert.Contains(returnedRoles, r => r.Name == "User");
        Assert.Contains(returnedRoles, r => r.Name == "Moderator");
    }

    [Fact]
    public async Task GetUserRoles_WhenUserHasNoRoles_ReturnsEmptyList()
    {
        // Arrange
        var userId = "user-123";
        var roles = new List<RoleDto>();

        _mockAuthService.Setup(s => s.GetUserRolesWithDetailsAsync(userId)).ReturnsAsync(roles);

        // Act
        var result = await _controller.GetUserRoles(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedRoles = Assert.IsAssignableFrom<IEnumerable<RoleDto>>(okResult.Value);
        Assert.Empty(returnedRoles);
    }

    [Fact]
    public async Task AddUserToRole_WhenSuccessful_ReturnsOkResult()
    {
        // Arrange
        var userId = "user-123";
        var roleName = "Manager";

        _mockAuthService.Setup(s => s.AddUserToRoleAsync(userId, roleName)).ReturnsAsync(true);

        // Act
        var result = await _controller.AddUserToRole(userId, roleName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var success = Assert.IsType<bool>(okResult.Value);
        Assert.True(success);
    }

    [Fact]
    public async Task AddUserToRole_WhenUserOrRoleNotFound_ReturnsOkWithFalse()
    {
        // Arrange
        var userId = "non-existent-user";
        var roleName = "Manager";

        _mockAuthService.Setup(s => s.AddUserToRoleAsync(userId, roleName)).ReturnsAsync(false);

        // Act
        var result = await _controller.AddUserToRole(userId, roleName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var success = Assert.IsType<bool>(okResult.Value);
        Assert.False(success);
    }

    [Fact]
    public async Task RemoveUserFromRole_WhenSuccessful_ReturnsOkResult()
    {
        // Arrange
        var userId = "user-123";
        var roleName = "Manager";

        _mockAuthService.Setup(s => s.RemoveUserFromRoleAsync(userId, roleName)).ReturnsAsync(true);

        // Act
        var result = await _controller.RemoveUserFromRole(userId, roleName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var success = Assert.IsType<bool>(okResult.Value);
        Assert.True(success);
    }

    [Fact]
    public async Task RemoveUserFromRole_WhenUserOrRoleNotFound_ReturnsOkWithFalse()
    {
        // Arrange
        var userId = "non-existent-user";
        var roleName = "Manager";

        _mockAuthService.Setup(s => s.RemoveUserFromRoleAsync(userId, roleName)).ReturnsAsync(false);

        // Act
        var result = await _controller.RemoveUserFromRole(userId, roleName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var success = Assert.IsType<bool>(okResult.Value);
        Assert.False(success);
    }

    // Additional Edge Cases and Error Handling Tests
    [Fact]
    public async Task Login_WhenServiceThrowsException_ExceptionPropagates()
    {
        // Arrange
        var loginRequest = new LoginRequestDto
        {
            Model = new LoginModel
            {
                Login = "test@example.com",
                Password = "TestPassword123!",
                InternalAuth = true,
            },
        };

        _mockAuthService.Setup(s => s.LoginAsync(loginRequest))
                       .ThrowsAsync(new InvalidOperationException("Database connection failed"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.Login(loginRequest));
    }

    [Fact]
    public async Task Login_WithExternalAuth_ReturnsSuccessResult()
    {
        // Arrange
        var loginRequest = new LoginRequestDto
        {
            Model = new LoginModel
            {
                Login = "test@example.com",
                Password = "TestPassword123!",
                InternalAuth = false, // External auth
            },
        };

        var authResult = new AuthResultDto
        {
            Success = true,
            Token = "external-jwt-token",
            UserId = "user-123",
            Email = "test@example.com",
            IsExternalUser = true,
        };

        _mockAuthService.Setup(s => s.LoginAsync(loginRequest)).ReturnsAsync(authResult);

        // Act
        var result = await _controller.Login(loginRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedResult = Assert.IsType<AuthResultDto>(okResult.Value);
        Assert.True(returnedResult.Success);
        Assert.True(returnedResult.IsExternalUser);
        Assert.Equal("external-jwt-token", returnedResult.Token);
    }

    [Fact]
    public async Task ValidateToken_WithMissingAuthorizationHeader_ReturnsInvalidToken()
    {
        // Arrange - No Authorization header
        var context = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = context,
        };

        _mockAuthService.Setup(s => s.ValidateTokenAsync(string.Empty)).ReturnsAsync(false);

        // Act
        var result = await _controller.ValidateToken();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var isValid = Assert.IsType<bool>(okResult.Value);
        Assert.False(isValid);
    }

    [Fact]
    public async Task ValidateToken_WithMalformedAuthorizationHeader_ReturnsInvalidToken()
    {
        // Arrange - Malformed Authorization header
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "InvalidHeader";
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = context,
        };

        _mockAuthService.Setup(s => s.ValidateTokenAsync("InvalidHeader")).ReturnsAsync(false);

        // Act
        var result = await _controller.ValidateToken();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var isValid = Assert.IsType<bool>(okResult.Value);
        Assert.False(isValid);
    }

    [Fact]
    public async Task CheckPageAccess_WithNullTargetPage_CallsServiceWithNull()
    {
        // Arrange
        var accessRequest = new AccessRequestDto
        {
            TargetPage = null,
            TargetId = Guid.NewGuid(),
            TargetKey = "test-key",
        };

        _mockAuthService.Setup(s => s.CheckPageAccessAsync(null, accessRequest.TargetId, accessRequest.TargetKey))
                       .ReturnsAsync(false);

        // Act
        var result = await _controller.CheckPageAccess(accessRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var hasAccess = Assert.IsType<bool>(okResult.Value);
        Assert.False(hasAccess);
        _mockAuthService.Verify(s => s.CheckPageAccessAsync(null, accessRequest.TargetId, accessRequest.TargetKey), Times.Once);
    }

    [Fact]
    public async Task CheckPageAccess_WhenServiceThrowsException_ExceptionPropagates()
    {
        // Arrange
        var accessRequest = new AccessRequestDto
        {
            TargetPage = "users",
            TargetId = Guid.NewGuid(),
            TargetKey = "test-key",
        };

        _mockAuthService.Setup(s => s.CheckPageAccessAsync(accessRequest.TargetPage, accessRequest.TargetId, accessRequest.TargetKey))
                       .ThrowsAsync(new UnauthorizedAccessException("Access denied"));

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _controller.CheckPageAccess(accessRequest));
    }

    [Fact]
    public async Task AddUser_WithNullUserData_ReturnsBadRequest()
    {
        // Arrange
        var addUserRequest = new AddUserRequestDto
        {
            User = null,
            Password = "TestPassword123!",
            Roles = new List<Guid>(),
        };

        var addResult = new AddUserResultDto
        {
            Success = false,
            Messages = new List<string> { "User data is required" },
        };

        _mockAuthService.Setup(s => s.AddUserAsync(addUserRequest)).ReturnsAsync(addResult);

        // Act
        var result = await _controller.AddUser(addUserRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var returnedResult = Assert.IsType<AddUserResultDto>(badRequestResult.Value);
        Assert.False(returnedResult.Success);
        Assert.Contains("User data is required", returnedResult.Messages);
    }

    [Fact]
    public async Task AddUser_WhenServiceThrowsException_ExceptionPropagates()
    {
        // Arrange
        var addUserRequest = new AddUserRequestDto
        {
            User = new CreateUserDto { Name = "Test User" },
            Password = "TestPassword123!",
            Roles = new List<Guid> { Guid.NewGuid() },
        };

        _mockAuthService.Setup(s => s.AddUserAsync(addUserRequest))
                       .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.AddUser(addUserRequest));
    }

    [Fact]
    public async Task UpdateUser_WithNullUserData_ReturnsOkWithFalse()
    {
        // Arrange
        var updateRequest = new UpdateUserRequestDto
        {
            User = null,
            Password = "NewPassword123!",
            Roles = new List<Guid>(),
        };

        _mockAuthService.Setup(s => s.UpdateUserAsync(null, updateRequest.Password, updateRequest.Roles))
                       .ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateUser(updateRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var success = Assert.IsType<bool>(okResult.Value);
        Assert.False(success);
    }

    [Fact]
    public async Task UpdateUser_WhenServiceThrowsException_ExceptionPropagates()
    {
        // Arrange
        var updateRequest = new UpdateUserRequestDto
        {
            User = new ReturnUserDto { Id = "user-123", Name = "Updated User" },
            Password = "NewPassword123!",
            Roles = new List<Guid>(),
        };

        _mockAuthService.Setup(s => s.UpdateUserAsync(updateRequest.User, updateRequest.Password, updateRequest.Roles))
                       .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.UpdateUser(updateRequest));
    }

    [Fact]
    public async Task DeleteUser_WithEmptyId_ReturnsOkWithFalse()
    {
        // Arrange
        var userId = string.Empty;
        _mockAuthService.Setup(s => s.DeleteUserAsync(userId)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var success = Assert.IsType<bool>(okResult.Value);
        Assert.False(success);
        _mockAuthService.Verify(s => s.DeleteUserAsync(userId), Times.Once);
    }

    [Fact]
    public async Task DeleteUser_WhenServiceThrowsException_ExceptionPropagates()
    {
        // Arrange
        var userId = "user-123";
        _mockAuthService.Setup(s => s.DeleteUserAsync(userId))
                       .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.DeleteUser(userId));
    }

    [Fact]
    public async Task GetUserRoles_WhenServiceThrowsException_ExceptionPropagates()
    {
        // Arrange
        var userId = "user-123";
        _mockAuthService.Setup(s => s.GetUserRolesWithDetailsAsync(userId))
                       .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetUserRoles(userId));
    }

    [Fact]
    public async Task AddUserToRole_WhenServiceThrowsException_ExceptionPropagates()
    {
        // Arrange
        var userId = "user-123";
        var roleName = "Manager";
        _mockAuthService.Setup(s => s.AddUserToRoleAsync(userId, roleName))
                       .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.AddUserToRole(userId, roleName));
    }

    [Fact]
    public async Task RemoveUserFromRole_WhenServiceThrowsException_ExceptionPropagates()
    {
        // Arrange
        var userId = "user-123";
        var roleName = "Manager";
        _mockAuthService.Setup(s => s.RemoveUserFromRoleAsync(userId, roleName))
                       .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.RemoveUserFromRole(userId, roleName));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task AddUserToRole_WithInvalidRoleName_ReturnsOkWithFalse(string roleName)
    {
        // Arrange
        var userId = "user-123";
        _mockAuthService.Setup(s => s.AddUserToRoleAsync(userId, roleName)).ReturnsAsync(false);

        // Act
        var result = await _controller.AddUserToRole(userId, roleName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var success = Assert.IsType<bool>(okResult.Value);
        Assert.False(success);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task RemoveUserFromRole_WithInvalidRoleName_ReturnsOkWithFalse(string roleName)
    {
        // Arrange
        var userId = "user-123";
        _mockAuthService.Setup(s => s.RemoveUserFromRoleAsync(userId, roleName)).ReturnsAsync(false);

        // Act
        var result = await _controller.RemoveUserFromRole(userId, roleName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var success = Assert.IsType<bool>(okResult.Value);
        Assert.False(success);
    }
}