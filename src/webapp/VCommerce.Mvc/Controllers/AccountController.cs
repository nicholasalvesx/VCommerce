using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(IAuthService authService,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _authService = authService;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(ModelState);

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

        await _signInManager.SignInAsync(user, model.RememberMe);

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
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}