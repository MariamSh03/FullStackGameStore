using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPanel.Entity;

public class GamePlatformEntity
{
    [Required]
    [ForeignKey("GameId")]
    public Guid GameId { get; set; }

    [Required]
    [ForeignKey("PlatformId")]
    public Guid PlatformId { get; set; }
}
