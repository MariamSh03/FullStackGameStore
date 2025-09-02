using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Bll.DTOs.Authentification;
public class AccessRequestDto
{
    [Required]
    public string TargetPage { get; set; }

    // Support both Guid IDs and string keys
    public Guid? TargetId { get; set; }

    public string? TargetKey { get; set; }
}