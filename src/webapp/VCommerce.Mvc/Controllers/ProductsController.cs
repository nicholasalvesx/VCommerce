using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    public ProductsController(IProductService productService,
                            ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductViewModel>>> Index()
    {
        
        var result = await _productService.GetAllProducts(await GetAccessToken());

        if (result is null)
            return View("Error");

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> CreateProduct()
    {
        ViewBag.CategoryId = new SelectList(await _categoryService.GetAllCategories(await GetAccessToken()), "CategoryId", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductViewModel? productVm)
    {
        if (!ModelState.IsValid) 
            return View(productVm);
        
        var result = await _productService.CreateProduct(productVm, await GetAccessToken());

        if (result!.Id > 0)
            return RedirectToAction(nameof(Index));
        
        ViewBag.CategoryId = new SelectList(await
            _categoryService.GetAllCategories(await GetAccessToken()), "CategoryId", "Name");

        return View(productVm);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateProduct(int id)  
    {
        ViewBag.CategoryId = new SelectList(await
                           _categoryService.GetAllCategories(await GetAccessToken()), "CategoryId", "Name");

        var result = await _productService.FindProductById(id, await GetAccessToken());

        return result is null ? View("Error") : View(result);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProduct(ProductViewModel productVm)
    {
        if (ModelState.IsValid)
        {
            var result = await _productService.UpdateProduct(productVm, await GetAccessToken());

            if (result != null!)
                return RedirectToAction(nameof(Index));
        }
        return View(productVm);
    }

    [HttpGet]
    public async Task<ActionResult<ProductViewModel>> DeleteProduct(int id)
    {
        var result = await _productService.FindProductById(id, await GetAccessToken());

        return result is null ? View("Error") : View(result);
    }

    [HttpPost, ActionName("DeleteProduct")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _productService.DeleteProductById(id, await GetAccessToken());

        if (!result)
            return View("Error");

        return RedirectToAction("Index");
    }
    private async Task<string?> GetAccessToken()
    {
        return await HttpContext.GetTokenAsync("access_token");
    }
}
