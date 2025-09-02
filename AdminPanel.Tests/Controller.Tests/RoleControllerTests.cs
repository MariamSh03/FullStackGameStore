using AdminPanel.Bll.DTOs.Authentification;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AdminPanel.Tests.Controller.Tests;

public class RoleControllerTests : IDisposable
{
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly RoleController _controller;

    public RoleControllerTests()
    {
        _mockAuthService = new Mock<IAuthService>();
        _controller = new RoleController(_mockAuthService.Object);
    }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    {
        // Controllers don't implement IDisposable in this case
    }

    [Fact]
    public async Task GetAllRoles_WhenRolesExist_ReturnsOkWithRoles()
    {
        // Arrange
        var roles = new List<ReturnRoleDto>
        {
            new() { Id = "role1", Name = "Admin" },
            new() { Id = "role2", Name = "Manager" },
            new() { Id = "role3", Name = "User" },
        };

        _mockAuthService.Setup(s => s.GetAllRolesAsync()).ReturnsAsync(roles);

        // Act
        var result = await _controller.GetAllRoles();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedRoles = Assert.IsAssignableFrom<IEnumerable<ReturnRoleDto>>(okResult.Value);
        Assert.Equal(3, returnedRoles.Count());
        Assert.Contains(returnedRoles, r => r.Name == "Admin");
        Assert.Contains(returnedRoles, r => r.Name == "Manager");
        Assert.Contains(returnedRoles, r => r.Name == "User");
    }

    [Fact]
    public async Task GetAllRoles_WhenNoRolesExist_ReturnsEmptyList()
    {
        // Arrange
        var roles = new List<ReturnRoleDto>();
        _mockAuthService.Setup(s => s.GetAllRolesAsync()).ReturnsAsync(roles);

        // Act
        var result = await _controller.GetAllRoles();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedRoles = Assert.IsAssignableFrom<IEnumerable<ReturnRoleDto>>(okResult.Value);
        Assert.Empty(returnedRoles);
    }

    [Fact]
    public async Task GetRoleById_WhenRoleExists_ReturnsOkWithRole()
    {
        // Arrange
        var roleId = "role-123";
        var role = new ReturnRoleDto { Id = roleId, Name = "Admin" };

        _mockAuthService.Setup(s => s.GetRoleByIdAsync(roleId)).ReturnsAsync(role);

        // Act
        var result = await _controller.GetRoleById(roleId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedRole = Assert.IsType<ReturnRoleDto>(okResult.Value);
        Assert.Equal(roleId, returnedRole.Id);
        Assert.Equal("Admin", returnedRole.Name);
    }

    [Fact]
    public async Task GetRoleById_WhenRoleDoesNotExist_ReturnsOkWithNull()
    {
        // Arrange
        var roleId = "non-existent-role";
        _mockAuthService.Setup(s => s.GetRoleByIdAsync(roleId)).ReturnsAsync((ReturnRoleDto)null!);

        // Act
        var result = await _controller.GetRoleById(roleId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Null(okResult.Value);
    }

    [Fact]
    public async Task DeleteRoleById_WhenRoleExists_ReturnsOkWithSuccessMessage()
    {
        // Arrange
        var roleId = "role-123";
        _mockAuthService.Setup(s => s.DeleteRoleByIdAsync(roleId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteRoleById(roleId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;

        // Check if the response contains the success message
        var messageProperty = response.GetType().GetProperty("message");
        Assert.NotNull(messageProperty);
        Assert.Equal("Role deleted successfully.", messageProperty.GetValue(response));

        // Verify that the service method was called
        _mockAuthService.Verify(s => s.DeleteRoleByIdAsync(roleId), Times.Once);
    }

    [Fact]
    public async Task DeleteRoleById_WhenServiceThrowsException_ExceptionPropagates()
    {
        // Arrange
        var roleId = "role-123";
        _mockAuthService.Setup(s => s.DeleteRoleByIdAsync(roleId))
                       .ThrowsAsync(new InvalidOperationException("Cannot delete role"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.DeleteRoleById(roleId));
    }

    // GetAllPermissions Tests
    [Fact]
    public async Task GetAllPermissions_WhenPermissionsExist_ReturnsOkWithPermissions()
    {
        // Arrange
        var permissions = new List<string>
        {
            "ManageUsers",
            "ViewUsers",
            "ManageRoles",
            "ViewRoles",
            "ManageGames",
        };

        _mockAuthService.Setup(s => s.GetAllPermissionsAsync()).ReturnsAsync(permissions);

        // Act
        var result = await _controller.GetAllPermissions();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedPermissions = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
        Assert.Equal(5, returnedPermissions.Count());
        Assert.Contains("ManageUsers", returnedPermissions);
        Assert.Contains("ViewUsers", returnedPermissions);
        Assert.Contains("ManageRoles", returnedPermissions);
    }

    [Fact]
    public async Task GetAllPermissions_WhenNoPermissionsExist_ReturnsEmptyList()
    {
        // Arrange
        var permissions = new List<string>();
        _mockAuthService.Setup(s => s.GetAllPermissionsAsync()).ReturnsAsync(permissions);

        // Act
        var result = await _controller.GetAllPermissions();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedPermissions = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
        Assert.Empty(returnedPermissions);
    }

    // GetRolePermissions Tests
    [Fact]
    public async Task GetRolePermissions_WhenRoleHasPermissions_ReturnsOkWithPermissions()
    {
        // Arrange
        var roleId = "admin-role";
        var permissions = new List<string>
        {
            "ManageUsers",
            "ViewUsers",
            "ManageRoles",
            "ViewRoles",
        };

        _mockAuthService.Setup(s => s.GetRolePermissionsAsync(roleId)).ReturnsAsync(permissions);

        // Act
        var result = await _controller.GetRolePermissions(roleId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedPermissions = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
        Assert.Equal(4, returnedPermissions.Count());
        Assert.Contains("ManageUsers", returnedPermissions);
        Assert.Contains("ViewUsers", returnedPermissions);
    }

    [Fact]
    public async Task GetRolePermissions_WhenRoleHasNoPermissions_ReturnsEmptyList()
    {
        // Arrange
        var roleId = "guest-role";
        var permissions = new List<string>();

        _mockAuthService.Setup(s => s.GetRolePermissionsAsync(roleId)).ReturnsAsync(permissions);

        // Act
        var result = await _controller.GetRolePermissions(roleId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedPermissions = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
        Assert.Empty(returnedPermissions);
    }

    // AddRole Tests
    [Fact]
    public async Task AddRole_WhenRoleAddedSuccessfully_ReturnsOkWithRole()
    {
        // Arrange
        var addRoleRequest = new AddRoleRequestDto
        {
            Role = new RoleDto { Name = "CustomRole" },
            Permissions = new List<string> { "ViewGames", "CommentOnGames" },
        };

        var addedRole = new RoleDto { Name = "CustomRole" };

        _mockAuthService.Setup(s => s.AddRoleAsync(addRoleRequest)).ReturnsAsync(addedRole);

        // Act
        var result = await _controller.AddRole(addRoleRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedRole = Assert.IsType<RoleDto>(okResult.Value);
        Assert.Equal("CustomRole", returnedRole.Name);
    }

    [Fact]
    public async Task AddRole_WhenServiceThrowsException_ExceptionPropagates()
    {
        // Arrange
        var addRoleRequest = new AddRoleRequestDto
        {
            Role = new RoleDto { Name = "DuplicateRole" },
            Permissions = new List<string>(),
        };

        _mockAuthService.Setup(s => s.AddRoleAsync(addRoleRequest))
                       .ThrowsAsync(new InvalidOperationException("Role already exists"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.AddRole(addRoleRequest));
    }

    // UpdateRole Tests
    [Fact]
    public async Task UpdateRole_WhenRoleUpdatedSuccessfully_ReturnsOkWithRole()
    {
        // Arrange
        var updateRoleRequest = new UpdateRoleRequestDto
        {
            Id = "role-123",
            Role = new RoleDto { Name = "UpdatedRoleName" },
            Permissions = new List<string> { "ViewGames", "ManageGames" },
        };

        var updatedRole = new RoleDto { Name = "UpdatedRoleName" };

        _mockAuthService.Setup(s => s.UpdateRoleAsync(updateRoleRequest)).ReturnsAsync(updatedRole);

        // Act
        var result = await _controller.UpdateRole(updateRoleRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedRole = Assert.IsType<RoleDto>(okResult.Value);
        Assert.Equal("UpdatedRoleName", returnedRole.Name);
    }

    [Fact]
    public async Task UpdateRole_WhenRoleNotFound_ServiceHandlesError()
    {
        // Arrange
        var updateRoleRequest = new UpdateRoleRequestDto
        {
            Id = "non-existent-role",
            Role = new RoleDto { Name = "UpdatedRoleName" },
            Permissions = new List<string>(),
        };

        _mockAuthService.Setup(s => s.UpdateRoleAsync(updateRoleRequest))
                       .ThrowsAsync(new ArgumentException("Role not found"));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _controller.UpdateRole(updateRoleRequest));
    }

    // Edge Cases and Error Handling Tests
    [Fact]
    public async Task GetAllRoles_WhenServiceThrowsException_ExceptionPropagates()
    {
        // Arrange
        _mockAuthService.Setup(s => s.GetAllRolesAsync())
                       .ThrowsAsync(new InvalidOperationException("Database connection failed"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetAllRoles());
    }

    [Fact]
    public async Task GetRoleById_WithEmptyId_CallsServiceWithEmptyId()
    {
        // Arrange
        var emptyId = string.Empty;
        _mockAuthService.Setup(s => s.GetRoleByIdAsync(emptyId)).ReturnsAsync((ReturnRoleDto)null!);

        // Act
        var result = await _controller.GetRoleById(emptyId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Null(okResult.Value);
        _mockAuthService.Verify(s => s.GetRoleByIdAsync(emptyId), Times.Once);
    }

    [Fact]
    public async Task AddRole_WithNullRequest_ServiceHandlesNull()
    {
        // Arrange
        AddRoleRequestDto? nullRequest = null;
        _mockAuthService.Setup(s => s.AddRoleAsync(nullRequest!))
                       .ThrowsAsync(new ArgumentNullException(nameof(nullRequest)));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _controller.AddRole(nullRequest!));
    }
}