using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Bll.DTOs.Authentification;

public class CreateUserDto
{
    [Required]
    public string Name { get; set; }
}