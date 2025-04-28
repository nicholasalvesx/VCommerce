using Microsoft.AspNetCore.Mvc;
using VCommerce.Api.DTOs;
using VCommerce.Api.Services;

namespace VCommerce.Api.Controllers;

[ApiController]
[Route("api/v1/categories")]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
    {
        var categoriesDto = await categoryService.GetCategories();
        if (categoriesDto == null!)
        {
            return NotFound("Categorias nao encontradas");    
        }
        return Ok(categoriesDto);
    }

    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesProducts()
    {
        var categoriesDto = await categoryService.GetCategoriesProduct();
        if (categoriesDto == null!)
        {
            return NotFound("Categorias e seus respectivos produtos nao foram encontradas");
        }
        return Ok(categoriesDto);
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
    {
        var categoryDto = await categoryService.GetCategoryById(id);
        if (categoryDto == null!)
        {
            return NotFound("Categoria nao encontrada");
        }
        return Ok(categoryDto);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> CreteCategory(CategoryDTO categoryDto)
    {
        if (categoryDto == null!)
        {
            return BadRequest("A categoria desse item nao existe");
        }
        
        await categoryService.AddCategory(categoryDto);
        
        return new CreatedAtRouteResult("GetCategory", new { id = categoryDto.CategoryId }, categoryDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryDTO>> UpdateCategory(int id, CategoryDTO categoryDto)
    {
        if (id != categoryDto.CategoryId || categoryDto == null!)
        {
            return BadRequest();
        }

        await categoryService.UpdateCategory(categoryDto);
        return Ok(categoryDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CategoryDTO>> DeleteCategory(int id)
    {
        var categoryDto = await categoryService.GetCategoryById(id);
        if (categoryDto == null!)
        {
            return NotFound("Categoria nao encontrada");
        }
        
        await categoryService.DeleteCategory(id);
        return Ok(categoryDto);
    }
}