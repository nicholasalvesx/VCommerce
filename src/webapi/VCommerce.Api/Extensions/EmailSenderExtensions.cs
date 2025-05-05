using System.Text.Encodings.Web;
using VCommerce.Api.Services;

namespace VCommerce.Api.Extensions;

public static class EmailSenderExtensions
{
    public static Task SendEmailConfirmation(this IEmailSender emailSender, string email, string? link)
    {
        return emailSender.SendEmail(email, "Confirmar seu email",
            $"Por favor, confirme seu e-mail clicando no: <a href='{HtmlEncoder.Default.Encode(link!)}'>link</a>");
    }
}