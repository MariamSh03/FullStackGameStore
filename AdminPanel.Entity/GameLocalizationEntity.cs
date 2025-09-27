using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPanel.Entity;
public class GameLocalizationEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public Guid GameId { get; set; }

    [ForeignKey("GameId")]
    public GameEntity Game { get; set; }

    [Required]
    [MaxLength(10)]
    public string Language { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    public string? Description { get; set; }
}
