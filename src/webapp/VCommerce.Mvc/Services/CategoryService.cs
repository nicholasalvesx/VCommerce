using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Services;

public class CategoryService : ICategoryService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions _options;
    private const string apiEndpoint = "/api/v1/categories/";
    private CategoryViewModel? _categoryVm;
    private IEnumerable<CategoryViewModel>? _categoriesVm;

    public CategoryService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<CategoryViewModel>?> GetAllCategories(string? token)
    {
        var client = _clientFactory.CreateClient("Api");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWTToken", token);

        IEnumerable<CategoryViewModel>? categories;

        using (var response = await client.GetAsync(apiEndpoint))
        {

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                categories = await JsonSerializer
                          .DeserializeAsync<IEnumerable<CategoryViewModel>>(apiResponse, _options);
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }
        }
        return categories;
    }
    
    public async Task<CategoryViewModel?> FindCategoryById(int id, string? token)
    {
        var categories = _clientFactory.CreateClient("Api");
        PutTokenInHeaderAuthorization(token, categories);

        using (var response = await categories.GetAsync("/api/v1/categories/" + id))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                _categoryVm = await JsonSerializer
                    .DeserializeAsync<CategoryViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return _categoryVm;
    }
    
    public async Task<CategoryViewModel?> CreateCategory(CategoryViewModel? category,string? token)
    {
        var categories = _clientFactory.CreateClient("Api");
        PutTokenInHeaderAuthorization(token, categories);

        var content = new StringContent(JsonSerializer.Serialize(category),
            Encoding.UTF8, "application/json");

        using var response = await categories.PostAsync("/api/v1/categories/", content);
        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStreamAsync();
            category = await JsonSerializer
                .DeserializeAsync<CategoryViewModel>(apiResponse, _options);
        }
        else
        {
            return null;
        }

        return category;
    }

    public async Task<CategoryViewModel?> UpdateCategory(CategoryViewModel? categoryVm, string? token)
    {
        var customer = _clientFactory.CreateClient("Api");
        PutTokenInHeaderAuthorization(token, customer);

        CategoryViewModel? categoryUpdated;

        using var response = await customer.PutAsJsonAsync("/api/v1/categories/", categoryVm);
        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStreamAsync();
            categoryUpdated = await JsonSerializer
                .DeserializeAsync<CategoryViewModel>(apiResponse, _options);
        }
        else
        {
            return null;
        }

        return categoryUpdated;
    }
    
    public async Task<CategoryViewModel?> GetCategoriesProducts(ProductViewModel product, string? token)
    {
        var category = _clientFactory.CreateClient("Api");
        PutTokenInHeaderAuthorization(token, category);
        
        using var response = await category.PutAsJsonAsync("/api/v1/categories/products", product);
        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStreamAsync();
            _categoryVm = await JsonSerializer
                .DeserializeAsync<CategoryViewModel>(apiResponse, _options);
        }
        else
        {
            return null;
        }

        return _categoryVm;
    }

    
    public async Task<bool> DeleteCategory(int id, string? token)
    {
        var categories = _clientFactory.CreateClient("Api");
        PutTokenInHeaderAuthorization(token, categories);

        using var response = await categories.DeleteAsync("/api/v1/categories/" + id);
        
        return response.IsSuccessStatusCode;
    }
    
    private static void PutTokenInHeaderAuthorization(string? token, HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("acess_token", token);
    }
}
