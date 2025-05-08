using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions _options;
    private ProductViewModel? _productVm;
    private IEnumerable<ProductViewModel>? _productsVm;

    public ProductService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<ProductViewModel>?> GetAllProducts(string? token)
    {
        var client = _clientFactory.CreateClient("Api");
        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.GetAsync("/api/v1/products"))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                _productsVm = await JsonSerializer
                            .DeserializeAsync<IEnumerable<ProductViewModel>>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return _productsVm;
    }

    public async Task<ProductViewModel?> FindProductById(int id, string? token)
    {
        var client = _clientFactory.CreateClient("Api");
        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.GetAsync("/api/v1/products/" + id))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                _productVm = await JsonSerializer
                          .DeserializeAsync<ProductViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return _productVm;
    }

    public async Task<ProductViewModel?> CreateProduct(ProductViewModel? productVm, string? token)
    {
        var client = _clientFactory.CreateClient("Api");
        PutTokenInHeaderAuthorization(token, client);

        var content = new StringContent(JsonSerializer.Serialize(productVm),
                                                  Encoding.UTF8, "application/json");

        using var response = await client.PostAsync("/api/v1/products/", content);
        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStreamAsync();
            productVm = await JsonSerializer
                .DeserializeAsync<ProductViewModel>(apiResponse, _options);
        }
        else
        {
            return null;
        }

        return productVm;
    }

    public async Task<ProductViewModel?> UpdateProduct(ProductViewModel productVm, string? token)
    {
        var client = _clientFactory.CreateClient("Api");
        PutTokenInHeaderAuthorization(token, client);

        ProductViewModel? productUpdated;

        using var response = await client.PutAsJsonAsync($"/api/v1/products/{productVm.Id}", productVm);
        
        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStreamAsync();
            productUpdated = await JsonSerializer
                .DeserializeAsync<ProductViewModel>(apiResponse, _options);
        }
        else
        {
            return null;
        }

        return productUpdated;
    }

    public async Task<bool> DeleteProductById(int id, string? token)
    {
        var client = _clientFactory.CreateClient("Api");
        PutTokenInHeaderAuthorization(token, client);

        using (var response = await client.DeleteAsync("/api/v1/products/" + id))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }
        return false;
    }

    private static void PutTokenInHeaderAuthorization(string? token, HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization =
                   new AuthenticationHeaderValue("acess_token", token);
    }
}
