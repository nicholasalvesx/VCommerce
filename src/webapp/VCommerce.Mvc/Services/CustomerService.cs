using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Services;

public class CustomerService : ICustomerService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions _options;
    private CustomerViewModel? _customerVm;
    private IEnumerable<CustomerViewModel>? _customersVm;
    
    public CustomerService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<CustomerViewModel>?> GetAllCustomers(string? token)
    {
        var customer = _clientFactory.CreateClient("Api");
        PutTokenInHeaderAuthorization(token, customer);

        using (var response = await customer.GetAsync("/api/v1/customers"))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                _customersVm = await JsonSerializer
                    .DeserializeAsync<IEnumerable<CustomerViewModel>>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return _customersVm;
    }

    public async Task<CustomerViewModel?> FindCustomerById(int id, string? token)
    {
        var customer = _clientFactory.CreateClient("Api");
        PutTokenInHeaderAuthorization(token, customer);

        using (var response = await customer.GetAsync("/api/v1/customers/" + id))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                _customerVm = await JsonSerializer
                    .DeserializeAsync<CustomerViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return _customerVm;
    }
    public async Task<CustomerViewModel?> CreateCustomer(CustomerViewModel? customerVm)
        {
            var customer = _clientFactory.CreateClient("Api");

            var content = new StringContent(JsonSerializer.Serialize(customerVm),
                Encoding.UTF8, "application/json");

            using var response = await customer.PostAsync("/api/v1/customers/", content);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                customerVm = await JsonSerializer
                    .DeserializeAsync<CustomerViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }

            return customerVm;
        }

        public async Task<CustomerViewModel?> UpdateCustomer(CustomerViewModel? customerVm, string? token)
        {
            var customer = _clientFactory.CreateClient("Api");
            PutTokenInHeaderAuthorization(token, customer);

            CustomerViewModel? customerUpdated;

            using var response = await customer.PutAsJsonAsync("/api/v1/customers/", customerVm);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                customerUpdated = await JsonSerializer
                    .DeserializeAsync<CustomerViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }

            return customerUpdated;
        }

        public async Task<bool> DeleteCustomerById(int id, string? token)
        {
            var customer = _clientFactory.CreateClient("Api");
            PutTokenInHeaderAuthorization(token, customer);

            using var response = await customer.DeleteAsync("/api/v1/customers/" + id);
        
            return response.IsSuccessStatusCode;
        }
    
        private static void PutTokenInHeaderAuthorization(string? token, HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("acess_token", token);
        }
}