using System.Net.Http.Json;
using AdminPanel.Bll.Configuration;
using AdminPanel.Bll.DTOs.Authentification;
using AdminPanel.Bll.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AdminPanel.Bll.Services;

public class ExternalAuthService : IExternalAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ExternalAuthConfig _externalAuthConfig;
    private readonly ILogger<ExternalAuthService> _logger;

    public ExternalAuthService(
        IHttpClientFactory httpClientFactory,
        IOptions<ExternalAuthConfig> externalAuthOptions,
        ILogger<ExternalAuthService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _externalAuthConfig = externalAuthOptions.Value;
        _logger = logger;
    }

    public async Task<bool> LoginAsync(LoginModelDto model)
    {
        try
        {
            if (string.IsNullOrEmpty(_externalAuthConfig.BaseUrl))
            {
                _logger.LogError("External auth configuration is missing BaseUrl");
                return false;
            }

            using var httpClient = _httpClientFactory.CreateClient("ExternalAuthClient");

            var authRequest = new
            {
                Login = model.Login,
                Password = model.Password,
                InternalAuth = model.InternalAuth,
            };

            _logger.LogInformation("External auth login attempt for user: {Login}", model.Login);

            var response = await httpClient.PostAsJsonAsync(_externalAuthConfig.LoginEndpoint, authRequest);
            var success = response.IsSuccessStatusCode;

            if (!success)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogWarning(
                    "External auth login failed for user: {Login}. Status: {StatusCode}, Response: {Response}",
                    model.Login,
                    response.StatusCode,
                    errorContent);
            }
            else
            {
                _logger.LogInformation("External auth login successful for user: {Login}", model.Login);
            }

            return success;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error during external auth login for user: {Login}", model.Login);
            return false;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout during external auth login for user: {Login}", model.Login);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during external auth login for user: {Login}", model.Login);
            return false;
        }
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            if (string.IsNullOrEmpty(_externalAuthConfig.BaseUrl))
            {
                _logger.LogError("External auth configuration is missing BaseUrl");
                return false;
            }

            using var httpClient = _httpClientFactory.CreateClient("ExternalAuthClient");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            _logger.LogDebug("Validating token with external auth service");

            var response = await httpClient.GetAsync(_externalAuthConfig.ValidateEndpoint);
            var isValid = response.IsSuccessStatusCode;

            if (!isValid)
            {
                _logger.LogWarning("Token validation failed. Status: {StatusCode}", response.StatusCode);
            }

            return isValid;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error during token validation");
            return false;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout during token validation");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during token validation");
            return false;
        }
    }
}