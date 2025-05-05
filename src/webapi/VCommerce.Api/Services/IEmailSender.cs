using SendGrid;

namespace VCommerce.Api.Services;

public interface IEmailSender
{
    Task<Response> SendEmail(string email, string subject, string message);
}