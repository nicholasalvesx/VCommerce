using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Controllers;

[Authorize]
public class CategoriesAppController : Controller
{
    private readonly ICategoryService _service;

    public CategoriesAppController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryViewModel>>> Index()
    {
        var result = await _service.GetAllCategories(await GetAccessToken());
        return View(result);
    }
    
    [HttpGet]
    public Task<ActionResult<IEnumerable<CategoryViewModel>>> CreateCategory()
    {
        return Task.FromResult<ActionResult<IEnumerable<CategoryViewModel>>>(View());
    }

    [HttpPost]
    public async Task<ActionResult<CategoryViewModel>> CreateCategory(CategoryViewModel? model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _service.CreateCategory(model!, await GetAccessToken());
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public Task<ActionResult<IEnumerable<CategoryViewModel>>> EditCategory(int id)
    {
        return Task.FromResult<ActionResult<IEnumerable<CategoryViewModel>>>(View());
    }

    [HttpPost]
    public async Task<ActionResult<CategoryViewModel>> EditCategory(CategoryViewModel? model)
    {
        if (!ModelState.IsValid)
            return View(model);
        
        var categoryUpdate = await _service.UpdateCategory(model, await GetAccessToken());
        return View(categoryUpdate ?? model);
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryViewModel>>> DeleteCategory(int id)
    {
        var result = await _service.FindCategoryById(id, await GetAccessToken());
        return View(result);
    }
    
    [HttpPost, ActionName("DeleteCategory")]
    public async Task<ActionResult<IEnumerable<CategoryViewModel>>> DeleteCategoryConfirmed(int id)
    {
        var result = await _service.DeleteCategory(id, await GetAccessToken());

        if (!result)
            return View("Error");

        return RedirectToAction("Index");
    }
    
    private async Task<string?> GetAccessToken()
    {
        return await HttpContext.GetTokenAsync("acess_token");
    }
}