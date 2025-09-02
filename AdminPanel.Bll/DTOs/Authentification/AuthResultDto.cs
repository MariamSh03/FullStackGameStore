namespace AdminPanel.Bll.DTOs.Authentification;

public class AuthResultDto
{
    public bool Success { get; set; }

    public string Message { get; set; }

    public string Token { get; set; }

    public string UserId { get; set; }

    public string Email { get; set; }

    public bool IsExternalUser { get; set; }
}