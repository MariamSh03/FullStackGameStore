namespace AdminPanel.Entity.Localization;

/// <summary>
/// Constants for supported languages in the system.
/// </summary>
public static class LanguageConstants
{
    /// <summary>
    /// English language code (default).
    /// </summary>
    public const string English = "en";

    /// <summary>
    /// Georgian language code.
    /// </summary>
    public const string Georgian = "geo";

    /// <summary>
    /// German language code.
    /// </summary>
    public const string German = "de";

    /// <summary>
    /// Default language used as fallback.
    /// </summary>
    public const string DefaultLanguage = English;

    /// <summary>
    /// List of all supported languages.
    /// </summary>
    public static readonly string[] SupportedLanguages = { English, Georgian, German };

    /// <summary>
    /// Checks if the provided language code is supported.
    /// </summary>
    /// <param name="languageCode">The language code to check.</param>
    /// <returns>True if supported, false otherwise.</returns>
    public static bool IsSupported(string languageCode)
    {
        return SupportedLanguages.Contains(languageCode, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets the default language if the provided language is not supported.
    /// </summary>
    /// <param name="languageCode">The language code to validate.</param>
    /// <returns>Valid language code or default language.</returns>
    public static string GetValidLanguageOrDefault(string languageCode)
    {
        return IsSupported(languageCode) ? languageCode.ToLowerInvariant() : DefaultLanguage;
    }
}
