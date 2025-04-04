using VCommerce.Mvc.Models;

namespace VCommerce.Mvc.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryViewModel>> GetCategories();
    Task<CategoryViewModel> GetCategoryProduct();
    Task<CategoryViewModel> GetCategoryById(int id);
    Task<CategoryViewModel> CreateCategory(CategoryViewModel category);
    Task<CategoryViewModel> UpdateCategory(CategoryViewModel category);
    Task<bool> DeleteCategory(int id);
}