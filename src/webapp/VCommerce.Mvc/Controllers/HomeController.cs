using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services;
using IProductService = VCommerce.Mvc.Services.Contracts.IProductService;

namespace VCommerce.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;

    public HomeController(ProductService productService)
    {
        _productService = productService;
    }

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
    [Authorize]
    public async Task<ActionResult<ProductViewModel>> ProductDetails(int id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var product = await _productService.FindProductById(id,token);

        if (product is null)
            return View("Error");

        return View(product);
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