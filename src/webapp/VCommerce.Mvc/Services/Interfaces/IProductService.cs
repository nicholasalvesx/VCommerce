using VCommerce.Mvc.Models;

namespace VCommerce.Mvc.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductViewModel>?> GetProducts();
    Task<ProductViewModel> GetProductById(int id);
    Task<ProductViewModel> CreateProduct(ProductViewModel product);
    Task<ProductViewModel> UpdateProduct(ProductViewModel product);
    Task<bool> DeleteProduct(int id);
}