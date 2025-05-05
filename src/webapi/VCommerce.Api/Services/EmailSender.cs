using SendGrid;
using SendGrid.Helpers.Mail;

namespace VCommerce.Api.Services;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Response> SendEmail(string email, string subject, string message)
    {
        var key = _configuration["SendGridKey"];

        var client = new SendGridClient(key);
        
        var msg = new SendGridMessage
        {
            From = new EmailAddress("naoresponda@vcommerce.com.br", "VCommerce"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        
        msg.AddTo(new EmailAddress(email));

        return await client.SendEmailAsync(msg);
    }
}
