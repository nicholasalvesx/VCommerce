using System.Text;
using System.Text.Json;
using VCommerce.Api.DTOs;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClientFactory.CreateClient("Api");
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AuthResult> LoginAsync(LoginViewModel loginModel)
    {
        var loginContent = new StringContent(
            JsonSerializer.Serialize(new LoginDTO
            {   
                Name = loginModel.Name,
                Password = loginModel.Password
            }), Encoding.UTF8,
            "application/json");
        
        var response = await _httpClient.PostAsync("/api/v1/auth/login", loginContent);

        if (!response.IsSuccessStatusCode)
            return new AuthResult { Succeeded = false };

        var content = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(content);
        var root = jsonDoc.RootElement;

        var token = root.GetProperty("token").GetString();
        var expiration = root.GetProperty("expirationToken").GetDateTime();

        if (string.IsNullOrEmpty(token))
            return new AuthResult { Succeeded = false };

        _httpContextAccessor.HttpContext?.Session.SetString("JWTToken", token);

        return new AuthResult
        {
            Succeeded = true,
            Token = token,
            Expiration = expiration
        };
    }
    
    public async Task<AuthResult?> RegisterAsync(RegisterViewModel registerModel)
    {
        var registerContent = new StringContent(
            JsonSerializer.Serialize(new
            {
                name = registerModel.Name,
                lastName = registerModel.LastName,
                email = registerModel.Email,
                password = registerModel.Password,
                confirmPassword = registerModel.ConfirmPassword
            }),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync("/api/v1/auth/register", registerContent);
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Resposta da API: " + content);

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        try
        {
            var result = JsonSerializer.Deserialize<AuthResult>(content, options);
            if (result == null || !result.Succeeded)
                return new AuthResult { Succeeded = false };

            if (!string.IsNullOrEmpty(result.Token))
            {
                _httpContextAccessor.HttpContext?.Session.SetString("JWTToken", result.Token);
            }

            return result;
        }
        catch (JsonException)
        {
            return new AuthResult { Succeeded = false };
        }
    }
    
    public void Logout()
    {
        _httpContextAccessor.HttpContext?.Session.Remove("JWT:SecretKey");
    }
}
    