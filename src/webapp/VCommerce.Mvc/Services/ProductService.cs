using System.Text.Json;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Interfaces;

namespace VCommerce.Mvc.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string apiEndpoint = "api/v1/products";
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private ProductViewModel? _product;
    private IEnumerable<ProductViewModel>? _products;

    public ProductService(JsonSerializerOptions jsonSerializerOptions, IHttpClientFactory httpClientFactory)
    {
        _jsonSerializerOptions = jsonSerializerOptions;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<ProductViewModel>?> GetProducts()
    {
        var client = _httpClientFactory.CreateClient("Api");

        using (var response = await client.GetAsync(apiEndpoint))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                
                _products = await JsonSerializer.DeserializeAsync<IEnumerable<ProductViewModel>>(apiResponse, _jsonSerializerOptions);
            }
            else
            {
                return null!;
            }
        }
        return _products;
    }

    public async Task<ProductViewModel> GetProductById(int id)
    {
        var client = _httpClientFactory.CreateClient("Api");

        using (var response = await client.GetAsync(apiEndpoint + id))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                
                _product = await JsonSerializer.DeserializeAsync<ProductViewModel>(apiResponse, _jsonSerializerOptions);
            }
            else
            {
                return null!;
            }
        }
        return _product!;
    }

    public async Task<ProductViewModel> CreateProduct(ProductViewModel product)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductViewModel> UpdateProduct(ProductViewModel product)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteProduct(int id)
    {
        throw new NotImplementedException();
    }
}