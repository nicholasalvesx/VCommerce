using VCommerce.Api.Models;

namespace VCommerce.Api.Repositores;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategories();
    
    Task<IEnumerable<Category>> GetCatgoriesProducts();
    
    Task<Category?> GetCategoryById(int id);
    
    Task<Category> CreateCategory(Category category);
    
    Task<Category> UpdateCategory(Category category);
    
    Task<Category> DeleteCategory(int id);
}