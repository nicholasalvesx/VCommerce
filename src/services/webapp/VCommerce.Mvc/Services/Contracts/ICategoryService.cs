using VCommerce.Mvc.Models;

namespace VCommerce.Mvc.Services.Contracts;

public interface ICategoryService
{
    Task<IEnumerable<CategoryViewModel>?> GetAllCategories(string? token);
    Task<CategoryViewModel?> FindCategoryById(int id, string? token);
    Task<CategoryViewModel?> CreateCategory(CategoryViewModel category ,string? token);
    Task<CategoryViewModel?> UpdateCategory(CategoryViewModel? categoryVm, string? token);
    Task<bool> DeleteCategory(int id, string? token);
}
