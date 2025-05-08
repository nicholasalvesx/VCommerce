using VCommerce.Modules.Core.Application.DTOs;

namespace VCommerce.Modules.Core.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetCategories();
    Task<IEnumerable<CategoryDto>> GetCategoriesProduct();
    Task<CategoryDto> GetCategoryById(int id);
    Task AddCategory(CategoryDto category);
    Task UpdateCategory(CategoryDto category);
    Task DeleteCategory(int id);
}