using VCommerce.Modules.Core.Domain.Entities;

namespace VCommerce.Modules.Core.Infra.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetCategories();
    
    Task<IEnumerable<Category>> GetCatgoriesProducts();
    
    Task<Category?> GetCategoryById(int id);
    
    Task<Category> CreateCategory(Category category);
    
    Task<Category> UpdateCategory(Category category);
    
    Task<Category> DeleteCategory(int id);
}