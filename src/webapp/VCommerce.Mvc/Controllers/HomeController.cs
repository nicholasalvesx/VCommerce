using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IAuthService _authService;
    private readonly IProductService _productService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IProductService productService, ILogger<HomeController> logger, IAuthService authService)
    {
        _productService = productService;
        _logger = logger;
        _authService = authService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllProducts(string.Empty);

        return products is null ? View("Error") : View(products);
    }

    [HttpGet]
    public async Task<IActionResult> ProductDetails(int id)
    {
        try
        {
            if (id <= 0)
            {
                TempData["Error"] = "ID do produto inválido.";
                return RedirectToAction(nameof(Index));
            }

            var token = await HttpContext.GetTokenAsync("access_token") 
                        ?? await HttpContext.GetTokenAsync("acess_token") 
                        ?? await HttpContext.GetTokenAsync("Bearer");

            _logger.LogInformation("Tentando acessar detalhes do produto ID: {ProductId}. Token obtido: {TokenStatus}", 
                id, !string.IsNullOrEmpty(token) ? "Sim" : "Não");

            if (string.IsNullOrEmpty(token) && User.Identity?.IsAuthenticated == true)
            {
                _logger.LogWarning("Usuário autenticado, mas token não encontrado. Tentando alternativas.");
            
                token = Request.Cookies["access_token"] ?? HttpContext.Session.GetString("access_token");
            
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("Token não encontrado em cookies ou sessão. Tentando prosseguir sem token.");
                }
            }

            ProductViewModel? product;
        
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogInformation("Tentando buscar produto sem token de autenticação");
                try
                {
                    product = await _productService.FindProductById(id, null);
                }
                catch (UnauthorizedAccessException)
                {
                    _logger.LogWarning("Acesso não autorizado ao tentar buscar produto sem token");
                    return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("ProductDetails", new { id }) });
                }
            }
            else
            {
                product = await _productService.FindProductById(id, token);
            }

            if (product is null)
            {
                TempData["Error"] = "Produto não encontrado.";
                return View("ProductNotFound"); 
            }

            product.Quantity = 1;
            return View(product);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro ao comunicar com a API de produtos. ID: {ProductId}. Status: {Status}", 
                id, ex.StatusCode);
            
            if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                TempData["Error"] = "Sua sessão expirou ou você não tem permissão para acessar este produto.";
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("ProductDetails", new { id }) });
            }
        
            TempData["Error"] = "Não foi possível conectar ao serviço de produtos. Tente novamente mais tarde.";
            return View("Error");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao buscar detalhes do produto. ID: {ProductId}", id);
            TempData["Error"] = "Ocorreu um erro inesperado. Por favor, tente novamente.";
            return View("Error");
        }
}

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(string message)
    {
        return View(new ErrorViewModel { ErrorMessage = message });
    }

    [Authorize]
    public async Task<IActionResult> Login()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");
        
        return string.IsNullOrEmpty(accessToken) ? 
            RedirectToAction("Login", "Account") : 
            RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Logout()
    {
        _authService.Logout();
        await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}