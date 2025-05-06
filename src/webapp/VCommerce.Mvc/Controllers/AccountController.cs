using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using VCommerce.Api.Models;
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

    public AccountController(IAuthService authService,
        UserManager<ApplicationUser> userManager, IEmailSender emailSender, ILogger<AccountController> logger)
    {
        _authService = authService;
        _userManager = userManager;
        _emailSender = emailSender;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl = null!)
    {
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ViewData["ReturnUrl"] = returnUrl;
        
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _authService.LoginAsync(model);
        
        var user = await _userManager.FindByNameAsync(model.Name!);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Esse cliente nao existe");
            return View(model);
        }

        var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password!);
        if (!passwordValid)
        {
            ModelState.AddModelError(string.Empty, "A senha esta invalida");
            return View(model);
        }

        if (!result.Succeeded)
        {
            await _userManager.AccessFailedAsync(user);
            ModelState.AddModelError(string.Empty, "Nao foi possivel realizar o login");
            return View(model);
        }
        
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
        
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register(string returnUrl = null!)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userExists = await _userManager.FindByEmailAsync(model.Email!);
        if (userExists != null)
        {
            ModelState.AddModelError(string.Empty, "Usuario ja existe");
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
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
    
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
    
        _logger.LogInformation($"Link de confirmação gerado: {callbackUrl}");
    
        if (model.Email != null) 
        {
            try
            {
                await _emailSender.SendEmailConfirmation(model.Email, callbackUrl);
                _logger.LogInformation("Email de confirmação enviado para: {ModelEmail}", model.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar email de confirmação: {ExMessage}", ex.Message);
            }
        }
    
        var customerResponse = await _authService.RegisterAsync(model);
        if (customerResponse is { Succeeded: false })
        {
            ModelState.AddModelError(string.Empty, "Tentativa de registro invalida");
            return View(model);
        }
    
        await _userManager.AddToRoleAsync(user, "Client");

        TempData["MSG_S"] = "Cadastro efetuado. Verifique seu email e confirme-o";
        ModelState.Clear();
    
        return View("AwaitingEmailConfirmation", model.Email);
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
    public async Task<IActionResult> Logout(string? returnUrl = null!)
    {
        _authService.Logout();
    
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    
        TempData["SuccessMessage"] = "Logout realizado com sucesso!";
    
        return RedirectToAction("Login", "Account");
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
            _logger.LogInformation($"Email confirmado com sucesso para usuário: {user.Email}");
            return View("Email");
        }
    
        _logger.LogWarning("Falha ao confirmar email para usuário: {UserEmail}. Erros: {Join}", user.Email, 
            string.Join(", ", result.Errors.Select(e => e.Description)));
        
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

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            TempData["MSG_E"] = "Usuário não encontrado";
            return RedirectToAction("Login");
        }

        if (user.EmailConfirmed)
        {
            TempData["MSG_S"] = "Seu email já foi confirmado. Faça login para continuar.";
            return RedirectToAction("Login");
        }

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
    
        try
        {
            await _emailSender.SendEmailConfirmation(email, callbackUrl);
            TempData["MSG_S"] = "Email de confirmação reenviado com sucesso";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao reenviar email de confirmação: {ExMessage}", ex.Message);
            TempData["MSG_E"] = "Erro ao reenviar email de confirmação. Tente novamente mais tarde.";
        }
    
        return View("AwaitingEmailConfirmation", email);
    }
}