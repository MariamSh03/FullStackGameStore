using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminPanel.Entity.Localization;

/// <summary>
/// Localization entity specifically for Publisher content.
/// </summary>
public class PublisherLocalizationEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets reference to the Publisher being localized.
    /// </summary>
    [Required]
    public Guid PublisherId { get; set; }

    /// <summary>
    /// Gets or sets language code (e.g., "en", "ua", "de").
    /// </summary>
    [Required]
    [MaxLength(10)]
    public string LanguageCode { get; set; }

    /// <summary>
    /// Gets or sets localized company name.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string CompanyName { get; set; }

    /// <summary>
    /// Gets or sets localized homepage description.
    /// </summary>
    public string? HomePage { get; set; }

    /// <summary>
    /// Gets or sets localized publisher description.
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
    public virtual PublisherEntity Publisher { get; set; }
}
