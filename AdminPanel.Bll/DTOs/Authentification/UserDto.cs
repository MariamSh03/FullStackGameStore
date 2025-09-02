namespace AdminPanel.Bll.DTOs.Authentification;

public class UserDto
{
    public string Id { get; set; }

    public string Email { get; set; }

    public string UserName { get; set; }

    public string Name { get; set; }

    public bool IsExternalUser { get; set; }
}
