namespace AdminPanel.Entity.Localization;

/// <summary>
/// Interface for entities that support localization.
/// </summary>
public interface ILocalizable
{
    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    Guid Id { get; }
}

/// <summary>
/// Interface for localization entities.
/// </summary>
public interface ILocalizationEntity
{
    /// <summary>
    /// Gets or sets the language code.
    /// </summary>
    string LanguageCode { get; set; }

    /// <summary>
    /// Gets or sets when this localization was created.
    /// </summary>
    DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets when this localization was last updated.
    /// </summary>
    DateTime? UpdatedAt { get; set; }
}
