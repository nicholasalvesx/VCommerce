using AutoMapper;
using VCommerce.Api.DTOs;
using VCommerce.Api.Models;
using VCommerce.Api.Repositores;

namespace VCommerce.Api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IMapper mapper, IProductRepository productRepository)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDTO>> GetProducts()
    {
        var productEntities = await _productRepository.GetProducts();
        return _mapper.Map<IEnumerable<ProductDTO>>(productEntities);
    }

    public async Task<IEnumerable<ProductDTO>> GetProductById(int id)
    {
        var productEntities = await _productRepository.GetProductById(id);
        return _mapper.Map<IEnumerable<ProductDTO>>(productEntities);
    }

    public async Task AddProduct(ProductDTO product)
    {
        var productEntity = _mapper.Map<Product>(product);
        await _productRepository.CreateProduct(productEntity);
        product.Id = productEntity.Id;
    }

    public async Task UpdateProduct(ProductDTO product)
    {
        var productEntity = _mapper.Map<Product>(product);
        await _productRepository.UpdateProduct(productEntity);
    }

    public async Task DeleteProduct(int id)
    {
        var productEntity = _productRepository.GetProductById(id);
        await _productRepository.DeleteProduct(productEntity.Id);
    }
}