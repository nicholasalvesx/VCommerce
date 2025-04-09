using System.Text.Json;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Interfaces;

namespace VCommerce.Mvc.Services;

public class CategoryService : ICategoryService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string apiEndpoint = "api/v1/categories";
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private IEnumerable<CategoryViewModel>? _categories;

    public CategoryService(IHttpClientFactory httpClientFactory, JsonSerializerOptions jsonSerializerOptions)
    {
        _httpClientFactory = httpClientFactory;
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    public async Task<IEnumerable<CategoryViewModel>> GetCategories()
    {
        var client = _httpClientFactory.CreateClient("Api");

        using (var response = await client.GetAsync(apiEndpoint))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiReponse = await response.Content.ReadAsStreamAsync();
                
                _categories = JsonSerializer.Deserialize<IEnumerable<CategoryViewModel>>(apiReponse, _jsonSerializerOptions);
            }
            else
            {
                return null!;
            }
        }
        return _categories!;
    }
}