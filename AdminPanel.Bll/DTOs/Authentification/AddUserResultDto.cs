namespace AdminPanel.Bll.DTOs.Authentification;

public class AddUserResultDto
{
    public bool Success { get; set; }

    public List<string> Messages { get; set; } = new();
}