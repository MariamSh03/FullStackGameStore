namespace AdminPanel.Bll.DTOs.Authentification;
public class UpdateUserRequestDto
{
    public ReturnUserDto User { get; set; }

    public List<Guid> Roles { get; set; }

    public string Password { get; set; }
}