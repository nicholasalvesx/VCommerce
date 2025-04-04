using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Interfaces;

namespace VCommerce.Mvc.Services;

public class CategoryService : ICategoryService
{
    public Task<IEnumerable<CategoryViewModel>> GetCategories()
    {
        throw new NotImplementedException();
    }

    public Task<CategoryViewModel> GetCategoryProduct()
    {
        throw new NotImplementedException();
    }

    public Task<CategoryViewModel> GetCategoryById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<CategoryViewModel> CreateCategory(CategoryViewModel category)
    {
        throw new NotImplementedException();
    }

    public Task<CategoryViewModel> UpdateCategory(CategoryViewModel category)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteCategory(int id)
    {
        throw new NotImplementedException();
    }
}