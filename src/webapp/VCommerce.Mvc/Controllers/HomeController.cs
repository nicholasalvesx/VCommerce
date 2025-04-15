using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ILogger _logger;

    public HomeController(IProductService productService, ILogger logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllProducts(string.Empty);

        if (products is null)
        {
            return View("Error");
        }

        return View(products);
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

            var token = await HttpContext.GetTokenAsync("access_token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("ProductDetails", new { id }) });
            }

            var product = await _productService.FindProductById(id, token);

            if (product is null)
            {
                TempData["Error"] = "Produto não encontrado.";
                return View();
            }

            product.Quantity = 1;

            return View(product);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro ao comunicar com a API de produtos. ID: {ProductId}", id);
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
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }
}