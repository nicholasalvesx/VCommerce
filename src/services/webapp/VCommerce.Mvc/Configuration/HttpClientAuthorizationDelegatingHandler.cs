using System.Net.Http.Headers;

namespace VCommerce.Mvc.Configuration;

public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HttpClientAuthorizationDelegatingHandler> _logger;

    public HttpClientAuthorizationDelegatingHandler(
        IHttpContextAccessor httpContextAccessor,
        ILogger<HttpClientAuthorizationDelegatingHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken");
        
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _logger.LogDebug("Token adicionado ao cabeçalho de autorização: {TokenPreview}", 
                token.Length > 10 ? token[..10] + "..." : token);
        }
        else
        {
            _logger.LogDebug("Nenhum token encontrado na sessão para requisição: {Method} {Url}", 
                request.Method, request.RequestUri);
        }

        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro na requisição HTTP: {Method} {Url} - {Message}", 
                request.Method, request.RequestUri, ex.Message);
            throw;
        }
    }
}