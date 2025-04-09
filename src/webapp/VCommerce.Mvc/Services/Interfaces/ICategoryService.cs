using VCommerce.Mvc.Models;

namespace VCommerce.Mvc.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryViewModel>> GetCategories();
}