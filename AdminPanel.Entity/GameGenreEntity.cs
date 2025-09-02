using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPanel.Entity;

public class GameGenreEntity
{
    [Required]
    [ForeignKey("GameId")]
    public Guid GameId { get; set; }

    [Required]
    [ForeignKey("GenreId")]
    public Guid GenreId { get; set; }
}
