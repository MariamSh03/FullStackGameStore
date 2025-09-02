using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPanel.Entity.Localization;

/// <summary>
/// Localization entity specifically for Genre content.
/// </summary>
public class GenreLocalizationEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets reference to the Genre being localized.
    /// </summary>
    [Required]
    public Guid GenreId { get; set; }

    /// <summary>
    /// Gets or sets language code (e.g., "en", "ua", "de").
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string LanguageCode { get; set; }

    /// <summary>
    /// Gets or sets localized genre name.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets when this localization was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets when this localization was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    public virtual GenreEntity Genre { get; set; }
}
