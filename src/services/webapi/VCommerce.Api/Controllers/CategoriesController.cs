using Microsoft.AspNetCore.Mvc;
using VCommerce.Modules.Core.Application.DTOs;
using VCommerce.Modules.Core.Application.Interfaces;

namespace VCommerce.Api.Controllers;

[ApiController]
[Route("api/v1/categories")]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categoriesDto = await categoryService.GetCategories();
        if (categoriesDto == null!)
        {
            return NotFound("Categorias nao encontradas");    
        }
        return Ok(categoriesDto);
    }

    [HttpGet("products")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategoriesProducts()
    {
        var categoriesDto = await categoryService.GetCategoriesProduct();
        if (categoriesDto == null!)
        {
            return NotFound("Categorias e seus respectivos produtos nao foram encontradas");
        }
        return Ok(categoriesDto);
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
    {
        var categoryDto = await categoryService.GetCategoryById(id);
        if (categoryDto == null!)
        {
            return NotFound("Categoria nao encontrada");
        }
        return Ok(categoryDto);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreteCategory(CategoryDto categoryDto)
    {
        if (categoryDto == null!)
        {
            return BadRequest("A categoria desse item nao existe");
        }
        
        await categoryService.AddCategory(categoryDto);
        
        return new CreatedAtRouteResult("GetCategory", new { id = categoryDto.CategoryId }, categoryDto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, CategoryDto categoryDto)
    {
        if (id != categoryDto.CategoryId || categoryDto == null!)
        {
            return BadRequest();
        }

        await categoryService.UpdateCategory(categoryDto);
        return Ok(categoryDto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CategoryDto>> DeleteCategory(int id)
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