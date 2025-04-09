using VCommerce.Api.DTOs;

namespace VCommerce.Api.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetCategories();
    Task<IEnumerable<CategoryDTO>> GetCategoriesProduct();
    Task<CategoryDTO> GetCategoryById(int id);
    Task AddCategory(CategoryDTO category);
    Task UpdateCategory(CategoryDTO category);
    Task DeleteCategory(int id);
}