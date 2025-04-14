using System.Text;
using System.Text.Json;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClientFactory.CreateClient("IdentityApi");
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AuthResult> LoginAsync(LoginViewModel loginModel)
    {
        var loginContent = new StringContent(
            JsonSerializer.Serialize(new { email = loginModel.Email, password = loginModel.Password }),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync("/api/v1/auth/login", loginContent);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<AuthResult>(content, options);

            if (result != null && !string.IsNullOrEmpty(result.Token))
            {
                _httpContextAccessor.HttpContext?.Session.SetString("JWTToken", result.Token);
                return result;
            }
        }

        return new AuthResult { Succeeded = false };
    }

    public async Task<AuthResult> RegisterAsync(RegisterViewModel registerModel)
    {
        var registerContent = new StringContent(
            JsonSerializer.Serialize(new
            {
                firstName = registerModel.FirstName,
                lastName = registerModel.LastName,
                email = registerModel.Email,
                password = registerModel.Password,
                confirmPassword = registerModel.ConfirmPassword
            }),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync("/api/v1/auth/register", registerContent);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<AuthResult>(content, options);

            if (result != null && !string.IsNullOrEmpty(result.Token))
            {
                _httpContextAccessor.HttpContext?.Session.SetString("JWTToken", result.Token);
                return result;
            }
        }

        return new AuthResult { Succeeded = false };
    }

    public void Logout()
    {
        _httpContextAccessor.HttpContext?.Session.Remove("JWTToken");
    }
}
    