using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
{
    ViewData["ReturnUrl"] = returnUrl;

    if (!ModelState.IsValid) 
        return View(model);
    
    var result = await _authService.LoginAsync(model);
        
    if (result.Succeeded)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, model.Name!),
        };
            
        if (result.UserId > 0)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()));
        }
            
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
            
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = model.RememberMe,
            ExpiresUtc = result.Expiration,
            AllowRefresh = true
        };
            
        authProperties.StoreTokens([
            new AuthenticationToken
            {
                Name = "access_token",
                Value = result.Token!
            },
            new AuthenticationToken
            {
                Name = "expires_at",
                Value = result.Expiration.ToString("o")
            }
        ]);
            
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authProperties);
            
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
            
        return RedirectToAction("Index", "Home");
    }
        
    ModelState.AddModelError(string.Empty, "Tentativa de login inválida.");

    return View(model);
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

        var result = await _authService.RegisterAsync(model);

        if (result is { Succeeded: true })
        {
            TempData["SuccessMessage"] = "Cadastro realizado com sucesso! Faça login para continuar.";
            return RedirectToAction("Login", "Account");
        }
        
        ModelState.AddModelError(string.Empty, "Erro ao registrar. Verifique seus dados e tente novamente.");
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        _authService.Logout();
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}