namespace AdminPanel.Bll.DTOs.Authentification;
public class AddRoleRequestDto
{
    public RoleDto Role { get; set; }

    public List<string> Permissions { get; set; }
}