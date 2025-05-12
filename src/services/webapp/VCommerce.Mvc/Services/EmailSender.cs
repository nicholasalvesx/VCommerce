using SendGrid;
using SendGrid.Helpers.Mail;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Services;

public partial class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
    {
        _configuration = configuration;
        _logger = logger;
        
        var apiKey = _configuration["SENDGRID_API_KEY"];
        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogError("SendGrid API Key não configurada");
        }
        else
        {
            var maskedKey = apiKey.Length > 8 
                ? apiKey[..4] + "..." + apiKey[^4..] 
                : "***";
            _logger.LogInformation("SendGrid API Key configurada: {MaskedKey}", maskedKey);
        }
    }

    public async Task SendEmail(string email, string? subject, string message)
    {
        try
        {
            var response = await Execute(email, subject, message);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Email enviado com sucesso para {Email}. StatusCode: {ResponseStatusCode}", email, response.StatusCode);
            }
            else
            {
                _logger.LogError("Falha ao enviar email para {Email}. StatusCode: {ResponseStatusCode}", email, response.StatusCode);
                var responseBody = await response.Body.ReadAsStringAsync();
                _logger.LogError("Resposta do SendGrid: {ResponseBody}", responseBody);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exceção ao enviar email para {Email}: {ExMessage}", email, ex.Message);
            throw;
        }
    }

    private async Task<Response> Execute(string email, string? subject, string message)
    {
        var apiKey = _configuration["SENDGRID_API_KEY"];
        
        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogError("SendGrid API Key não configurada");
            throw new InvalidOperationException("SendGrid API Key não configurada");
        }

        var client = new SendGridClient(apiKey);
        
        var fromEmail = _configuration["SendGrid:FromEmail"] ?? "nicholas.alves2007@gmail.com";
        var fromName = _configuration["SendGrid:FromName"] ?? "VCommerce";
        
        _logger.LogInformation("Enviando email de {FromEmail} ({FromName}) para {Email}", fromEmail, fromName, email);
        
        var from = new EmailAddress(fromEmail, fromName);
        var to = new EmailAddress(email);
        
        var msg = new SendGridMessage
        {
            From = from,
            Subject = subject,
            PlainTextContent = StripHtml(message),
            HtmlContent = message
        };
        
        msg.AddTo(to);
        
        msg.SetClickTracking(true, true);
        msg.SetOpenTracking(true);
        msg.SetSubscriptionTracking(false);
        msg.SetGoogleAnalytics(false);
        
        return await client.SendEmailAsync(msg);
    }
    
    private static string StripHtml(string html)
    {
        return MyRegex().Replace(html, string.Empty)
            .Replace("&nbsp;", " ")
            .Replace("&amp;", "&")
            .Replace("&lt;", "<")
            .Replace("&gt;", ">");
    }

    [System.Text.RegularExpressions.GeneratedRegex("<.*?>")]
    private static partial System.Text.RegularExpressions.Regex MyRegex();
}