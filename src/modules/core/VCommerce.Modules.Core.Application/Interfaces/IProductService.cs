using VCommerce.Modules.Core.Application.DTOs;

namespace VCommerce.Modules.Core.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProducts();
    Task<ProductDTO> GetProductById(int id);
    Task AddProduct(ProductDTO? product);
    Task UpdateProduct(ProductDTO product);
    Task DeleteProduct(int id);
}