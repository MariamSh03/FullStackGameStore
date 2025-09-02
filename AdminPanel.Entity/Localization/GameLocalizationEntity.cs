using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPanel.Entity.Localization;

/// <summary>
/// Localization entity specifically for Game content.
/// </summary>
public class GameLocalizationEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets reference to the Game being localized.
    /// </summary>
    [Required]
    public Guid GameId { get; set; }

    /// <summary>
    /// Gets or sets language code (e.g., "en", "geo", "de").
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string LanguageCode { get; set; }

    /// <summary>
    /// Gets or sets localized game name.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets localized game description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets when this localization was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets when this localization was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    public virtual GameEntity Game { get; set; }
}
