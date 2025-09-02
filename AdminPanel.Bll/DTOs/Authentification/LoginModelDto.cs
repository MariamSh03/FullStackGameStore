namespace AdminPanel.Bll.DTOs.Authentification;

public class LoginModelDto
{
    public string Login { get; set; }

    public string Password { get; set; }

    public bool InternalAuth { get; set; }
}