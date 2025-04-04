using VCommerce.Api.DTOs;

namespace VCommerce.Api.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProducts();
    Task<IEnumerable<ProductDTO>> GetProductById(int id);
    Task AddProduct(ProductDTO product);
    Task UpdateProduct(ProductDTO product);
    Task DeleteProduct(int id);
}