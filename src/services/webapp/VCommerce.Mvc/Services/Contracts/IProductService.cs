using VCommerce.Mvc.Models;

namespace VCommerce.Mvc.Services.Contracts;

public interface IProductService
{
    Task<IEnumerable<ProductViewModel>?> GetAllProducts(string? token);
    Task<ProductViewModel?> FindProductById(int id, string? token);
    Task<ProductViewModel?> CreateProduct(ProductViewModel? productVm, string? token);
    Task<ProductViewModel?> UpdateProduct(ProductViewModel productVm, string? token);
    Task<bool> DeleteProductById(int id, string? token);
}
