using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPanel.Entity.Localization;

/// <summary>
/// Localization entity specifically for Platform content.
/// </summary>
public class PlatformLocalizationEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets reference to the Platform being localized.
    /// </summary>
    [Required]
    public Guid PlatformId { get; set; }

    /// <summary>
    /// Gets or sets language code (e.g., "en", "ua", "de").
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string LanguageCode { get; set; }

    /// <summary>
    /// Gets or sets localized platform type.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets when this localization was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets when this localization was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    public virtual PlatformEntity Platform { get; set; }
}
