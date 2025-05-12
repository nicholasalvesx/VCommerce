using System.Text;
using System.Text.Json;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;
using AuthResult = VCommerce.Mvc.Models.AuthResult;

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
            JsonSerializer.Serialize(new
            {
                name = loginModel.Name,
                password = loginModel.Password
            }), Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync("https://vcommerce-api-xd5k.onrender.com/api/v1/auth/login", loginContent);

        if (!response.IsSuccessStatusCode)
            return new AuthResult { Succeeded = false };

        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Resposta da API: " + content);
        
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
            if (result is not { Succeeded: true })
                return new AuthResult { Succeeded = false };
            
            return result;
        }
        catch (JsonException)
        {
            return new AuthResult { Succeeded = false };
        }
    }
    
    public void Logout()
    {
        var httpContext = _httpContextAccessor.HttpContext;
    
        if (httpContext != null)
        {
            httpContext.Session.Remove("JWTToken");
        
            httpContext.Session.Clear();
        
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
                HttpOnly = true,
                Secure = true 
            };
        
            httpContext.Response.Cookies.Delete("AuthCookie", cookieOptions);
        }
    }
}
    