using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using VCommerce.Api.Models;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(IAuthService authService,
        UserManager<ApplicationUser> userManager)
    {
        _authService = authService;
        _userManager = userManager;
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
            ModelState.AddModelError(string.Empty, "Tentativa de login invalida");
            return View(model);
        }

        var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password!);
        if (!passwordValid)
        {
            await _userManager.AccessFailedAsync(user);
            ModelState.AddModelError(string.Empty, "Tentativa de login invalida");
            return View(model);
        }

        if (!result.Succeeded)
        {
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
        
        var customerResponse = await _authService.RegisterAsync(model);
        if (customerResponse is { Succeeded: false })
        {
            ModelState.AddModelError(string.Empty, "Tentativa de registro invalida");
            return View(model);
        }
        
        var createdUser = await _userManager.FindByEmailAsync(model.Email!);
        if (createdUser == null)
        {
            ModelState.AddModelError(string.Empty, "Erro ao encontrar usuário criado.");
            return View(model);
        }

        await _userManager.AddToRoleAsync(createdUser, "Admin");

        TempData["MSG_S"] = "Cadastro efetuado";
        ModelState.Clear();
        
        return RedirectToAction("Login", "Account");
        
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        _authService.Logout();
        await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}