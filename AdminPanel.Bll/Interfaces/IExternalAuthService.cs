using AdminPanel.Bll.DTOs.Authentification;

namespace AdminPanel.Bll.Interfaces;

public interface IExternalAuthService
{
    Task<bool> LoginAsync(LoginModelDto model);

    Task<bool> ValidateTokenAsync(string token);
}
