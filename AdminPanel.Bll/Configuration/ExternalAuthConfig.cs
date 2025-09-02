namespace AdminPanel.Bll.Configuration;

public class ExternalAuthConfig
{
    public string BaseUrl { get; set; } = string.Empty;

    public string LoginEndpoint { get; set; } = "/api/auth";

    public string ValidateEndpoint { get; set; } = "/api/auth/validate";

    public int TimeoutSeconds { get; set; } = 30;

    public string GetLoginUrl() => $"{BaseUrl.TrimEnd('/')}{LoginEndpoint}";

    public string GetValidateUrl() => $"{BaseUrl.TrimEnd('/')}{ValidateEndpoint}";

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(BaseUrl) &&
               !string.IsNullOrWhiteSpace(LoginEndpoint) &&
               !string.IsNullOrWhiteSpace(ValidateEndpoint) &&
               TimeoutSeconds > 0 &&
               Uri.TryCreate(BaseUrl, UriKind.Absolute, out _);
    }

    public IEnumerable<string> GetValidationErrors()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(BaseUrl))
        {
            errors.Add("BaseUrl is required");
        }
        else if (!Uri.TryCreate(BaseUrl, UriKind.Absolute, out _))
        {
            errors.Add("BaseUrl must be a valid absolute URI");
        }

        if (string.IsNullOrWhiteSpace(LoginEndpoint))
        {
            errors.Add("LoginEndpoint is required");
        }

        if (string.IsNullOrWhiteSpace(ValidateEndpoint))
        {
            errors.Add("ValidateEndpoint is required");
        }

        if (TimeoutSeconds <= 0)
        {
            errors.Add("TimeoutSeconds must be greater than 0");
        }

        return errors;
    }
}