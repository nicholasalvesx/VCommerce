using System.Text;
using System.Text.Json;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IHttpClientFactory httpClientFactory, 
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("Api");
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<AuthResult> LoginAsync(LoginViewModel loginModel)
    {
        try
        {
            var loginContent = new StringContent(
                JsonSerializer.Serialize(new
                {
                    name = loginModel.Name,
                    password = loginModel.Password
                }), Encoding.UTF8, "application/json");
            
            _logger.LogInformation("Tentando autenticar usuário: {Username}", loginModel.Name);
            
            var response = await _httpClient.PostAsync("/api/v1/auth/login", loginContent);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Falha na autenticação. Status: {StatusCode}", response.StatusCode);
                return new AuthResult { Succeeded = false, Message = "Falha na autenticação. Verifique suas credenciais." };
            }

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogDebug("Resposta da API: {Content}", content);
            
            var jsonDoc = JsonDocument.Parse(content);
            var root = jsonDoc.RootElement;
            
            if (!root.TryGetProperty("token", out var tokenElement) || 
                !root.TryGetProperty("expirationToken", out var expirationElement))
            {
                _logger.LogWarning("Resposta da API não contém token ou data de expiração");
                return new AuthResult { Succeeded = false, Message = "Resposta inválida do servidor de autenticação." };
            }
            
            var token = tokenElement.GetString();
            var expiration = expirationElement.GetDateTime();
            
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Token recebido é nulo ou vazio");
                return new AuthResult { Succeeded = false, Message = "Token de autenticação inválido." };
            }

            _httpContextAccessor.HttpContext?.Session.SetString("JWTToken", token);
            _logger.LogInformation("Usuário autenticado com sucesso: {Username}", loginModel.Name);
            
            return new AuthResult
            {
                Succeeded = true,
                Token = token,
                Expiration = expiration,
                Message = "Autenticação realizada com sucesso."
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de conexão ao tentar autenticar: {Message}", ex.Message);
            return new AuthResult 
            { 
                Succeeded = false, 
                Message = "Não foi possível conectar ao servidor de autenticação. Verifique se o serviço está disponível."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado durante autenticação: {Message}", ex.Message);
            return new AuthResult 
            { 
                Succeeded = false, 
                Message = "Ocorreu um erro inesperado durante a autenticação."
            };
        }
    }
    
    public async Task<AuthResult> RegisterAsync(RegisterViewModel registerModel)
    {
        try
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
            
            _logger.LogInformation("Tentando registrar novo usuário: {Email}", registerModel.Email);
            
            var response = await _httpClient.PostAsync("/api/v1/auth/register", registerContent);
            var content = await response.Content.ReadAsStringAsync();
            
            _logger.LogDebug("Resposta da API de registro: {Content}", content);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Falha no registro. Status: {StatusCode}, Resposta: {Content}", 
                    response.StatusCode, content);
                
                return new AuthResult 
                { 
                    Succeeded = false, 
                    Message = "Falha no registro. " + (string.IsNullOrEmpty(content) ? 
                        "Verifique os dados informados." : content)
                };
            }
            
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            try
            {
                var result = JsonSerializer.Deserialize<AuthResult>(content, options);
                if (result == null || !result.Succeeded)
                {
                    _logger.LogWarning("Registro falhou após deserialização: {Content}", content);
                    return new AuthResult 
                    { 
                        Succeeded = false, 
                        Message = result?.Message ?? "Falha no registro do usuário."
                    };
                }
                
                _logger.LogInformation("Usuário registrado com sucesso: {Email}", registerModel.Email);
                return result;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Erro ao deserializar resposta de registro: {Content}", content);
                return new AuthResult 
                { 
                    Succeeded = false, 
                    Message = "Erro ao processar resposta do servidor de registro."
                };
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de conexão ao tentar registrar: {Message}", ex.Message);
            return new AuthResult 
            { 
                Succeeded = false, 
                Message = "Não foi possível conectar ao servidor de registro. Verifique se o serviço está disponível."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado durante registro: {Message}", ex.Message);
            return new AuthResult 
            { 
                Succeeded = false, 
                Message = "Ocorreu um erro inesperado durante o registro."
            };
        }
    }
    
    public void Logout()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        
        if (httpContext != null)
        {
            _logger.LogInformation("Realizando logout do usuário");
            
            httpContext.Session.Remove("JWTToken");
            
            httpContext.Session.Clear();
            
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            };
            
            httpContext.Response.Cookies.Delete("AuthCookie", cookieOptions);
            
            _logger.LogInformation("Logout realizado com sucesso");
        }
    }
}