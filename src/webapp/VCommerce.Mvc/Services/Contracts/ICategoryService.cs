using VCommerce.Mvc.Models;

namespace VCommerce.Mvc.Services.Contracts;

public interface ICategoryService
{
    Task<IEnumerable<CategoryViewModel>> GetAllCategories(string? token);
}
