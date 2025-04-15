using VCommerce.Mvc.Models;

namespace VCommerce.Mvc.Services.Contracts;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(LoginViewModel loginModel);
    Task<AuthResult?> RegisterAsync(RegisterViewModel registerModel);
    void Logout();
}