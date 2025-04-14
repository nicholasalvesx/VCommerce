using Microsoft.AspNetCore.Mvc;
using VCommerce.Api.DTOs;
using VCommerce.Api.Services;

namespace VCommerce.Api.Controllers;

[ApiController]
[Route("api/v1/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
    {
        var categoriesDto = await _categoryService.GetCategories();
        if (categoriesDto == null!)
        {
            return NotFound("Categories not found");    
        }
        return Ok(categoriesDto);
    }

    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesProducts()
    {
        var categoriesDto = await _categoryService.GetCategoriesProduct();
        if (categoriesDto == null!)
        {
            return NotFound("Categories not found");
        }
        return Ok(categoriesDto);
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
    {
        var categoryDto = await _categoryService.GetCategoryById(id);
        if (categoryDto == null!)
        {
            return NotFound("Category not found");
        }
        return Ok(categoryDto);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> CreteCategory(CategoryDTO categoryDto)
    {
        if (categoryDto == null!)
        {
            return BadRequest("Category object is null");
        }
        
        await _categoryService.AddCategory(categoryDto);
        
        return new CreatedAtRouteResult("GetCategory", new { id = categoryDto.CategoryId }, categoryDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryDTO>> UpdateCategory(int id, CategoryDTO categoryDto)
    {
        if (id != categoryDto.CategoryId)
        {
            return BadRequest();
        }

        if (categoryDto == null!)
        {
            return BadRequest();
        }
            
        await _categoryService.UpdateCategory(categoryDto);
        return Ok(categoryDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CategoryDTO>> DeleteCategory(int id)
    {
        var categoryDto = await _categoryService.GetCategoryById(id);
        if (categoryDto == null!)
        {
            return NotFound("Category not found");
        }
        
        await _categoryService.DeleteCategory(id);
        return Ok(categoryDto);
    }
    
    
}