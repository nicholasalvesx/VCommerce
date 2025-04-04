using VCommerce.Api.Models;

namespace VCommerce.Api.Repositores;

public interface IProductRepository
{
    
    Task<IEnumerable<Product>> GetProducts();
    
    Task<Product?> GetProductById(int id);
    
    Task<Product> CreateProduct(Product product);
    
    Task<Product> UpdateProduct(Product product);
    
    Task<Product?> DeleteProduct(int id);
}