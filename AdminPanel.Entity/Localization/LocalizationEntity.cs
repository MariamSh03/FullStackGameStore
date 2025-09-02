using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPanel.Entity.Localization;

/// <summary>
/// Base localization entity for storing translated content.
/// This table stores all localized text for different entities and languages.
/// </summary>
public class LocalizationEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the entity being localized (e.g., GameId, GenreId, etc.)
    /// </summary>
    [Required]
    public Guid EntityId { get; set; }

    /// <summary>
    /// Gets or sets the type of entity being localized (e.g., "Game", "Genre", "Publisher", etc.)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string EntityType { get; set; }

    /// <summary>
    /// Gets or sets the specific field being localized (e.g., "Name", "Description", etc.)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string FieldName { get; set; }

    /// <summary>
    /// Gets or sets language code (e.g., "en", "geo", "de").
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string LanguageCode { get; set; }

    /// <summary>
    /// Gets or sets The localized text content.
    /// </summary>
    [Required]
    public string Content { get; set; }

    /// <summary>
    /// Gets or sets when this localization was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets when this localization was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
