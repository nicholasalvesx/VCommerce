using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Interfaces;

namespace VCommerce.Mvc.Services;

public class ProductService : IProductService
{
    public Task<IEnumerable<ProductViewModel>> GetProducts()
    {
        throw new NotImplementedException();
    }

    public Task<ProductViewModel> GetProductById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ProductViewModel> CreateProduct(ProductViewModel product)
    {
        throw new NotImplementedException();
    }

    public Task<ProductViewModel> UpdateProduct(ProductViewModel product)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteProduct(int id)
    {
        throw new NotImplementedException();
    }
}