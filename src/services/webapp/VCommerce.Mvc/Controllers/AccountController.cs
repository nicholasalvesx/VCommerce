using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using VCommerce.Modules.Core.Infra.Models;
using VCommerce.Mvc.Extensions;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAuthService _authService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;

    public AccountController(
        IAuthService authService,
        UserManager<ApplicationUser> userManager, 
        IEmailSender emailSender, 
        ILogger<AccountController> logger)
    {
        _authService = authService;
        _userManager = userManager;
        _emailSender = emailSender;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string returnUrl = null)
    {
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        ViewData["ReturnUrl"] = returnUrl;
        
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        try
        {
            var user = await _userManager.FindByNameAsync(model.Name!);
            if (user == null)
            {
                _logger.LogWarning("Tentativa de login com usuário inexistente: {Username}", model.Name);
                ModelState.AddModelError(string.Empty, "Usuário não encontrado");
                return View(model);
            }

            if (!user.EmailConfirmed)
            {
                _logger.LogWarning("Tentativa de login com email não confirmado: {Username}", model.Name);
                TempData["WarningMessage"] = "Você precisa confirmar seu email antes de fazer login.";
                return RedirectToAction(nameof(AwaitingEmailConfirmation), new { email = user.Email });
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password!);
            if (!passwordValid)
            {
                _logger.LogWarning("Senha inválida para usuário: {Username}", model.Name);
                await _userManager.AccessFailedAsync(user);
                ModelState.AddModelError(string.Empty, "Senha inválida");
                return View(model);
            }

            var result = await _authService.LoginAsync(model);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Falha na autenticação via API: {Message}", result.Message);
                ModelState.AddModelError(string.Empty, result.Message ?? "Não foi possível realizar o login");
                return View(model);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email!)
            };
            
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            _logger.LogInformation("Usuário autenticado com sucesso: {Username}", model.Name);
            TempData["SuccessMessage"] = "Login realizado com sucesso!";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante o processo de login: {Message}", ex.Message);
            ModelState.AddModelError(string.Empty, "Ocorreu um erro durante o login. Tente novamente mais tarde.");
            return View(model);
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        try
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email!);
            if (userExists != null)
            {
                _logger.LogWarning("Tentativa de registro com email já existente: {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "Este email já está em uso");
                return View(model);
            }

            var user = new ApplicationUser
            {
                CustomerId = null,
                Email = model.Email,
                Name = model.Name,
                LastName = model.LastName,
                UserName = model.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, model.Password!);
            if (!result.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);

            _logger.LogInformation("Link de confirmação gerado: {CallbackUrl}", callbackUrl);

            if (model.Email != null)
            {
                try
                {
                    await _emailSender.SendEmailConfirmation(model.Email, callbackUrl);
                    _logger.LogInformation("Email de confirmação enviado para: {Email}", model.Email);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao enviar email de confirmação: {Message}", ex.Message);
                    TempData["WarningMessage"] = "Cadastro realizado, mas houve um problema ao enviar o email de confirmação.";
                }
            }

            var customerResponse = await _authService.RegisterAsync(model);
            if (customerResponse == null || !customerResponse.Succeeded)
            {
                _logger.LogWarning("Falha ao registrar cliente na API: {Message}", 
                    customerResponse?.Message ?? "Resposta nula");
                
                _logger.LogWarning("Usuário criado localmente, mas não foi registrado na API de clientes");
            }

            await _userManager.AddToRoleAsync(user, "Client");
            
            TempData["SuccessMessage"] = "Cadastro realizado com sucesso! Verifique seu email para confirmar sua conta.";
            ModelState.Clear();

            return RedirectToAction(nameof(AwaitingEmailConfirmation), new { email = model.Email });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante o processo de registro: {Message}", ex.Message);
            ModelState.AddModelError(string.Empty, "Ocorreu um erro durante o registro. Tente novamente mais tarde.");
            return View(model);
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AwaitingEmailConfirmation()
    {
        return View("AwaitingEmailConfirmation");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        if (User.Identity is { IsAuthenticated: false })
        {
            return RedirectToAction("Login", "Account");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(string returnUrl = null)
    {
        try
        {
            _authService.Logout();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _logger.LogInformation("Usuário deslogado com sucesso");
            TempData["SuccessMessage"] = "Logout realizado com sucesso!";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Login", "Account");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante o processo de logout: {Message}", ex.Message);
            TempData["ErrorMessage"] = "Ocorreu um erro durante o logout.";
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
        {
            _logger.LogWarning("Tentativa de confirmação de email com parâmetros inválidos");
            return View("Error", new ErrorViewModel { ErrorMessage = "Parâmetros inválidos para confirmação de email." });
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("Usuário não encontrado para ID: {UserId}", userId);
            return View("Error", new ErrorViewModel { ErrorMessage = "Usuário não encontrado." });
        }

        code = code.Replace(" ", "+");

        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            _logger.LogInformation("Email confirmado com sucesso para usuário: {UserEmail}", user.Email);
            TempData["SuccessMessage"] = "Email confirmado com sucesso! Agora você pode fazer login.";
            return View("EmailConfirmed");
        }

        _logger.LogWarning("Falha ao confirmar email para usuário: {UserEmail}. Erros: {Errors}", 
            user.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
            
        return View("Error", new ErrorViewModel { ErrorMessage = "Erro ao confirmar email. O link pode ter expirado." });
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ResendConfirmationEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email não fornecido");
        }

        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Tentativa de reenvio de email para usuário inexistente: {Email}", email);
                TempData["ErrorMessage"] = "Usuário não encontrado";
                return RedirectToAction("Login");
            }

            if (user.EmailConfirmed)
            {
                _logger.LogInformation("Tentativa de reenvio para email já confirmado: {Email}", email);
                TempData["SuccessMessage"] = "Seu email já foi confirmado. Faça login para continuar.";
                return RedirectToAction("Login");
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);

            try
            {
                await _emailSender.SendEmailConfirmation(email, callbackUrl);
                _logger.LogInformation("Email de confirmação reenviado com sucesso para: {Email}", email);
                TempData["SuccessMessage"] = "Email de confirmação reenviado com sucesso";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao reenviar email de confirmação: {Message}", ex.Message);
                TempData["ErrorMessage"] = "Erro ao reenviar email de confirmação. Tente novamente mais tarde.";
            }

            return View("AwaitingEmailConfirmation", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar reenvio de email: {Message}", ex.Message);
            TempData["ErrorMessage"] = "Ocorreu um erro ao processar sua solicitação.";
            return RedirectToAction("Login");
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }
}