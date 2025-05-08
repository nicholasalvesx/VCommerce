namespace VCommerce.Mvc.Services.Contracts;

public interface IEmailSender
{
    Task SendEmail(string email, string? subject, string message);
}