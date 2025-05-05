using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Extensions;

public static class EmailSenderExtensions
{
    public static Task SendEmailConfirmation(this IEmailSender emailSender, string email, string? link)
    {
        string emailTemplate = GetEmailConfirmationTemplate(email, link);
        return emailSender.SendEmail(email, "Confirme seu email - VCommerce", emailTemplate);
    }
    
    private static string GetEmailConfirmationTemplate(string email, string? link)
    {
        return $$"""

                 <!DOCTYPE html>
                 <html>
                 <head>
                     <meta charset='utf-8'>
                     <meta name='viewport' content='width=device-width, initial-scale=1'>
                     <title>Confirme seu email - VCommerce</title>
                     <style>
                         body, html {
                             margin: 0;
                             padding: 0;
                             font-family: Arial, Helvetica, sans-serif;
                             color: #333;
                             background-color: #f5f5f5;
                         }
                         .email-container {
                             max-width: 600px;
                             margin: 0 auto;
                             background-color: #ffffff;
                             border-radius: 8px;
                             overflow: hidden;
                         }
                         .email-header {
                             background-color: #4361ee;
                             padding: 30px;
                             text-align: center;
                         }
                         .email-header img {
                             max-width: 180px;
                             height: auto;
                         }
                         .email-body {
                             padding: 40px 30px;
                             text-align: center;
                         }
                         .email-title {
                             font-size: 24px;
                             font-weight: bold;
                             margin-bottom: 20px;
                             color: #333;
                         }
                         .email-message {
                             font-size: 16px;
                             line-height: 1.6;
                             margin-bottom: 30px;
                             color: #555;
                             text-align: left;
                         }
                         .email-button {
                             display: inline-block;
                             background-color: #4361ee;
                             color: #ffffff;
                             text-decoration: none;
                             padding: 14px 30px;
                             border-radius: 4px;
                             font-weight: bold;
                             font-size: 16px;
                             margin-bottom: 30px;
                         }
                         .email-note {
                             font-size: 14px;
                             color: #777;
                             margin-bottom: 20px;
                         }
                         .email-link {
                             word-break: break-all;
                             color: #4361ee;
                         }
                         .email-footer {
                             background-color: #f8f9fa;
                             padding: 20px 30px;
                             text-align: center;
                             font-size: 14px;
                             color: #777;
                             border-top: 1px solid #eee;
                         }
                     </style>
                 </head>
                 <body>
                     <div class='email-container'>
                         <div class='email-header'>
                             <i class="fa fa-shopping-bag" aria-hidden="true"></i>
                         </div>
                         <div class='email-body'>
                             <h1 class='email-title'>Confirme seu Email</h1>
                             <div class='email-message'>
                                 <p>Olá,</p>
                                 <p>Obrigado por se cadastrar na VCommerce! Para ativar sua conta e começar a usar nossa plataforma, 
                                 por favor confirme seu endereço de email clicando no botão abaixo:</p>
                             </div>
                             <a href='{{link}}' class='email-button'>Confirmar meu Email</a>
                             <p class='email-note'>
                                 Se o botão acima não funcionar, copie e cole o link abaixo no seu navegador:
                             </p>
                             <p class='email-link'>{{link}}</p>
                             <p class='email-note'>
                                 Este link expirará em 24 horas. Se você não solicitou este email, por favor ignore-o.
                             </p>
                         </div>
                         <div class='email-footer'>
                             <p>© 2025 VCommerce. Todos os direitos reservados.</p>
                             <p>Se você tiver alguma dúvida, entre em contato com nosso suporte: <a href='mailto:suporte@vcommerce.com.br'>suporte@vcommerce.com.br</a></p>
                         </div>
                     </div>
                 </body>
                 </html>
                 """;
    }
}