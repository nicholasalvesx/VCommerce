using SendGrid;

namespace VCommerce.Api.Services;

public interface IEmailSender
{
    Task SendEmail(string email, string? subject, string message);
}