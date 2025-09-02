namespace AdminPanel.Bll.DTOs.Authentification;
public class UpdateRoleRequestDto
{
    public string Id { get; set; }

    public RoleDto Role { get; set; }

    public List<string> Permissions { get; set; }
}