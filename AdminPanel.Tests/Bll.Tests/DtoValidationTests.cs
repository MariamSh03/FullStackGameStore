using System.ComponentModel.DataAnnotations;
using AdminPanel.Bll.DTOs.Authentification;

namespace AdminPanel.Tests.Bll.Tests;

public class DtoValidationTests
{
    [Fact]
    public void CreateUserDto_WithValidName_PassesValidation()
    {
        // Arrange
        var dto = new CreateUserDto
        {
            Name = "ValidUserName",
        };

        // Act
        var results = ValidateDto(dto);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void CreateUserDto_WithEmptyName_FailsValidation()
    {
        // Arrange
        var dto = new CreateUserDto
        {
            Name = string.Empty,
        };

        // Act
        var results = ValidateDto(dto);

        // Assert
        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.ErrorMessage?.Contains("required", StringComparison.OrdinalIgnoreCase) == true);
    }

    [Fact]
    public void CreateUserDto_WithNullName_FailsValidation()
    {
        // Arrange
        var dto = new CreateUserDto
        {
            Name = null,
        };

        // Act
        var results = ValidateDto(dto);

        // Assert
        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.ErrorMessage?.Contains("required", StringComparison.OrdinalIgnoreCase) == true);
    }

    [Fact]
    public void AccessRequestDto_WithValidTargetPage_PassesValidation()
    {
        // Arrange
        var dto = new AccessRequestDto
        {
            TargetPage = "Games",
            TargetId = Guid.NewGuid(),
            TargetKey = "test-key",
        };

        // Act
        var results = ValidateDto(dto);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void AccessRequestDto_WithEmptyTargetPage_FailsValidation()
    {
        // Arrange
        var dto = new AccessRequestDto
        {
            TargetPage = string.Empty,
            TargetId = Guid.NewGuid(),
            TargetKey = "test-key",
        };

        // Act
        var results = ValidateDto(dto);

        // Assert
        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.ErrorMessage?.Contains("required", StringComparison.OrdinalIgnoreCase) == true);
    }

    [Fact]
    public void AccessRequestDto_WithNullTargetPage_FailsValidation()
    {
        // Arrange
        var dto = new AccessRequestDto
        {
            TargetPage = null,
            TargetId = Guid.NewGuid(),
            TargetKey = "test-key",
        };

        // Act
        var results = ValidateDto(dto);

        // Assert
        Assert.NotEmpty(results);
        Assert.Contains(results, r => r.ErrorMessage?.Contains("required", StringComparison.OrdinalIgnoreCase) == true);
    }

    [Fact]
    public void AccessRequestDto_WithOnlyTargetPage_PassesValidation()
    {
        // Arrange
        var dto = new AccessRequestDto
        {
            TargetPage = "Users",
            TargetId = null,
            TargetKey = null,
        };

        // Act
        var results = ValidateDto(dto);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void AccessRequestDto_WithTargetIdOnly_PassesValidation()
    {
        // Arrange
        var dto = new AccessRequestDto
        {
            TargetPage = "Games",
            TargetId = Guid.NewGuid(),
            TargetKey = null,
        };

        // Act
        var results = ValidateDto(dto);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void AccessRequestDto_WithTargetKeyOnly_PassesValidation()
    {
        // Arrange
        var dto = new AccessRequestDto
        {
            TargetPage = "Games",
            TargetId = null,
            TargetKey = "game-key-123",
        };

        // Act
        var results = ValidateDto(dto);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void LoginRequestDto_StructureValidation()
    {
        // Arrange
        var dto = new LoginRequestDto
        {
            Model = new LoginModel
            {
                Login = "test@example.com",
                Password = "password123",
                InternalAuth = true,
            },
        };

        // Act & Assert
        Assert.NotNull(dto.Model);
        Assert.Equal("test@example.com", dto.Model.Login);
        Assert.Equal("password123", dto.Model.Password);
        Assert.True(dto.Model.InternalAuth);
    }

    [Fact]
    public void AuthResultDto_StructureValidation()
    {
        // Arrange
        var dto = new AuthResultDto
        {
            Success = true,
            Message = "Login successful",
            Token = "jwt-token-123",
            UserId = "user-123",
            Email = "test@example.com",
            IsExternalUser = false,
        };

        // Act & Assert
        Assert.True(dto.Success);
        Assert.Equal("Login successful", dto.Message);
        Assert.Equal("jwt-token-123", dto.Token);
        Assert.Equal("user-123", dto.UserId);
        Assert.Equal("test@example.com", dto.Email);
        Assert.False(dto.IsExternalUser);
    }

    [Fact]
    public void UserDto_StructureValidation()
    {
        // Arrange
        var dto = new UserDto
        {
            Id = "user-123",
            Email = "test@example.com",
            UserName = "testuser",
            Name = "Test User",
            IsExternalUser = false,
        };

        // Act & Assert
        Assert.Equal("user-123", dto.Id);
        Assert.Equal("test@example.com", dto.Email);
        Assert.Equal("testuser", dto.UserName);
        Assert.Equal("Test User", dto.Name);
        Assert.False(dto.IsExternalUser);
    }

    [Fact]
    public void ReturnUserDto_StructureValidation()
    {
        // Arrange
        var dto = new ReturnUserDto
        {
            Id = "user-123",
            Name = "Test User",
        };

        // Act & Assert
        Assert.Equal("user-123", dto.Id);
        Assert.Equal("Test User", dto.Name);
    }

    [Fact]
    public void RoleDto_StructureValidation()
    {
        // Arrange
        var dto = new RoleDto
        {
            Name = "Admin",
        };

        // Act & Assert
        Assert.Equal("Admin", dto.Name);
    }

    [Fact]
    public void ReturnRoleDto_StructureValidation()
    {
        // Arrange
        var dto = new ReturnRoleDto
        {
            Id = "role-123",
            Name = "Admin",
        };

        // Act & Assert
        Assert.Equal("role-123", dto.Id);
        Assert.Equal("Admin", dto.Name);
    }

    [Fact]
    public void AddUserRequestDto_StructureValidation()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var dto = new AddUserRequestDto
        {
            User = new CreateUserDto { Name = "New User" },
            Roles = new List<Guid> { roleId },
            Password = "password123",
        };

        // Act & Assert
        Assert.NotNull(dto.User);
        Assert.Equal("New User", dto.User.Name);
        Assert.Single(dto.Roles);
        Assert.Equal(roleId, dto.Roles[0]);
        Assert.Equal("password123", dto.Password);
    }

    [Fact]
    public void UpdateUserRequestDto_StructureValidation()
    {
        // Arrange
        var roleId = Guid.NewGuid();
        var dto = new UpdateUserRequestDto
        {
            User = new ReturnUserDto { Id = "user-123", Name = "Updated User" },
            Roles = new List<Guid> { roleId },
            Password = "newpassword123",
        };

        // Act & Assert
        Assert.NotNull(dto.User);
        Assert.Equal("user-123", dto.User.Id);
        Assert.Equal("Updated User", dto.User.Name);
        Assert.Single(dto.Roles);
        Assert.Equal(roleId, dto.Roles[0]);
        Assert.Equal("newpassword123", dto.Password);
    }

    [Fact]
    public void AddRoleRequestDto_StructureValidation()
    {
        // Arrange
        var dto = new AddRoleRequestDto
        {
            Role = new RoleDto { Name = "CustomRole" },
            Permissions = new List<string> { "ViewGames", "ManageGames" },
        };

        // Act & Assert
        Assert.NotNull(dto.Role);
        Assert.Equal("CustomRole", dto.Role.Name);
        Assert.Equal(2, dto.Permissions.Count);
        Assert.Contains("ViewGames", dto.Permissions);
        Assert.Contains("ManageGames", dto.Permissions);
    }

    [Fact]
    public void UpdateRoleRequestDto_StructureValidation()
    {
        // Arrange
        var dto = new UpdateRoleRequestDto
        {
            Id = "role-123",
            Role = new RoleDto { Name = "UpdatedRole" },
            Permissions = new List<string> { "ViewGames" },
        };

        // Act & Assert
        Assert.Equal("role-123", dto.Id);
        Assert.NotNull(dto.Role);
        Assert.Equal("UpdatedRole", dto.Role.Name);
        Assert.Single(dto.Permissions);
        Assert.Contains("ViewGames", dto.Permissions);
    }

    [Fact]
    public void AddUserResultDto_StructureValidation()
    {
        // Arrange
        var dto = new AddUserResultDto
        {
            Success = true,
            Messages = new List<string> { "User created successfully" },
        };

        // Act & Assert
        Assert.True(dto.Success);
        Assert.Single(dto.Messages);
        Assert.Contains("User created successfully", dto.Messages);
    }

    [Fact]
    public void AddUserResultDto_MessagesCollection_InitializesEmpty()
    {
        // Arrange & Act
        var dto = new AddUserResultDto();

        // Assert
        Assert.NotNull(dto.Messages);
        Assert.Empty(dto.Messages);
    }

    [Fact]
    public void AddUserRequestDto_EmptyRolesCollection_IsValid()
    {
        // Arrange
        var dto = new AddUserRequestDto
        {
            User = new CreateUserDto { Name = "Test User" },
            Roles = new List<Guid>(),
            Password = "password123",
        };

        // Act
        var results = ValidateDto(dto);

        // Assert
        Assert.Empty(results); // Should pass validation even with empty roles
        Assert.NotNull(dto.Roles);
        Assert.Empty(dto.Roles);
    }

    [Fact]
    public void AddRoleRequestDto_EmptyPermissionsCollection_IsValid()
    {
        // Arrange
        var dto = new AddRoleRequestDto
        {
            Role = new RoleDto { Name = "BasicRole" },
            Permissions = new List<string>(),
        };

        // Act
        var results = ValidateDto(dto);

        // Assert
        Assert.Empty(results); // Should pass validation even with empty permissions
        Assert.NotNull(dto.Permissions);
        Assert.Empty(dto.Permissions);
    }

    [Fact]
    public void LoginRequestDto_WithNullModel_DoesNotThrow()
    {
        // Arrange
        var dto = new LoginRequestDto
        {
            Model = null,
        };

        // Act & Assert
        Assert.Null(dto.Model);
    }

    [Fact]
    public void AddUserRequestDto_WithNullUser_DoesNotThrow()
    {
        // Arrange
        var dto = new AddUserRequestDto
        {
            User = null,
            Roles = new List<Guid>(),
            Password = "password123",
        };

        // Act & Assert
        Assert.Null(dto.User);
    }

    [Fact]
    public void UpdateUserRequestDto_WithNullUser_DoesNotThrow()
    {
        // Arrange
        var dto = new UpdateUserRequestDto
        {
            User = null,
            Roles = new List<Guid>(),
            Password = "password123",
        };

        // Act & Assert
        Assert.Null(dto.User);
    }

    [Fact]
    public void AccessRequestDto_WithMaxLengthTargetPage_PassesValidation()
    {
        // Arrange
        var longTargetPage = new string('A', 1000); // Very long string
        var dto = new AccessRequestDto
        {
            TargetPage = longTargetPage,
            TargetId = null,
            TargetKey = null,
        };

        // Act
        var results = ValidateDto(dto);

        // Assert
        Assert.Empty(results); // Should pass as no MaxLength attribute is defined
    }

    [Fact]
    public void AccessRequestDto_WithMaxLengthTargetKey_PassesValidation()
    {
        // Arrange
        var longTargetKey = new string('K', 1000); // Very long string
        var dto = new AccessRequestDto
        {
            TargetPage = "Games",
            TargetId = null,
            TargetKey = longTargetKey,
        };

        // Act
        var results = ValidateDto(dto);

        // Assert
        Assert.Empty(results); // Should pass as no MaxLength attribute is defined
    }

    [Fact]
    public void CreateUserDto_WithMaxLengthName_PassesValidation()
    {
        // Arrange
        var longName = new string('N', 1000); // Very long string
        var dto = new CreateUserDto
        {
            Name = longName,
        };

        // Act
        var results = ValidateDto(dto);

        // Assert
        Assert.Empty(results); // Should pass as no MaxLength attribute is defined
    }

    private static IList<ValidationResult> ValidateDto<T>(T dto)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        var context = new ValidationContext(dto, null, null);
#pragma warning restore CS8604 // Possible null reference argument.
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(dto, context, results, true);
        return results;
    }
}