using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Extensions;

public static class EmailSenderExtensions
{
    public static Task SendEmailConfirmation(this IEmailSender emailSender, string email, string? link)
    {
        var emailTemplate = GetEmailConfirmationTemplate(email, link);
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
                             font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                             color: #333;
                             background-color: transparent;
                         }
                         .email-container {
                             max-width: 600px;
                             margin: 20px auto;
                             background-color: rgba(255, 255, 255, 0.95);
                             border-radius: 12px;
                             overflow: hidden;
                             box-shadow: 0 4px 24px rgba(0, 0, 0, 0.08);
                         }
                         .email-header {
                             background-color: #4361ee;
                             padding: 30px;
                             text-align: center;
                             position: relative;
                         }
                         .email-logo {
                             display: inline-block;
                             background-color: white;
                             border-radius: 50px;
                             padding: 12px 20px;
                             font-weight: bold;
                             font-size: 22px;
                             color: #4361ee;
                         }
                         .email-logo-icon {
                             display: inline-block;
                             background-color: #4361ee;
                             color: white;
                             border-radius: 50%;
                             width: 28px;
                             height: 28px;
                             line-height: 28px;
                             text-align: center;
                             margin-right: 5px;
                         }
                         .email-body {
                             padding: 40px 30px;
                             text-align: center;
                             background-color: transparent;
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
                             padding: 16px 32px;
                             border-radius: 50px;
                             font-weight: bold;
                             font-size: 16px;
                             margin-bottom: 30px;
                             transition: background-color 0.2s;
                         }
                         .email-button:hover {
                             background-color: #3a53d0;
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
                             background-color: rgba(248, 249, 250, 0.7);
                             padding: 20px 30px;
                             text-align: center;
                             font-size: 14px;
                             color: #777;
                             border-top: 1px solid rgba(238, 238, 238, 0.5);
                         }
                         .divider {
                             height: 1px;
                             background-color: rgba(0, 0, 0, 0.05);
                             margin: 20px 0;
                         }
                     </style>
                 </head>
                 <body>
                     <div class='email-container'>
                         <div class='email-header'>
                             <div class='email-logo'>
                                 <span class='email-logo-icon'>V</span>commerce
                             </div>
                         </div>
                         <div class='email-body'>
                             <h1 class='email-title'>Confirme sua identidade</h1>
                             <div class='email-message'>
                                 <p>Olá,</p>
                                 <p>Agradecemos o registro. Para confirmar sua identidade, verifique seu e-mail clicando no botão abaixo:</p>
                             </div>
                             <a href='{{link}}' class='email-button'>Verificar meu e-mail</a>
                             <div class='divider'></div>
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
                             <p>Se você tiver alguma dúvida, entre em contato com nosso suporte: <a href='mailto:suporte@vcommerce.com.br' style='color: #4361ee;'>suporte@vcommerce.com.br</a></p>
                         </div>
                     </div>
                 </body>
                 </html>
                 """;
    }
}