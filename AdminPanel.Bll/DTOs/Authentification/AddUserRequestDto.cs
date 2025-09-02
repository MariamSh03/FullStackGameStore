namespace AdminPanel.Bll.DTOs.Authentification;
public class AddUserRequestDto
{
    // Match README: only user.name is required for creation
    public CreateUserDto User { get; set; }

    public List<Guid> Roles { get; set; }

    public string Password { get; set; }
}