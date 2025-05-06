using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Services;

public class OrderService : IOrderService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions _options;
    private OrderViewModel? _orderVm;
    private IEnumerable<OrderViewModel>? _ordersVm;


    public OrderService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<OrderViewModel>?> GetAllOrders(string? token)
    {
        var order = _clientFactory.CreateClient("Api");
        PutTokenInHeaderAuthorization(token, order);

        using (var response = await order.GetAsync("/api/v1/orders"))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                _ordersVm = await JsonSerializer
                    .DeserializeAsync<IEnumerable<OrderViewModel>>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return _ordersVm;
    }
    
    public async Task<OrderViewModel?> GetOrderById(int id, string? token)
    {
        var order = _clientFactory.CreateClient("Api");
        PutTokenInHeaderAuthorization(token, order);

        using (var response = await order.GetAsync("/api/v1/orders" + id))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                _orderVm = await JsonSerializer
                    .DeserializeAsync<OrderViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return _orderVm;
    }

    public async Task<OrderViewModel?> CreateOrder(OrderViewModel model, string? accessToken)
    {
        {
            var order = _clientFactory.CreateClient("Api");

            var content = new StringContent(JsonSerializer.Serialize(_orderVm),
                Encoding.UTF8, "application/json");

            using var response = await order.PostAsync("/api/v1/orders/", content);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                _orderVm = await JsonSerializer
                    .DeserializeAsync<OrderViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }

            return _orderVm;
        }
    }
        public async Task<OrderViewModel?> UpdateOrder(OrderViewModel model, string? token)
        {
            var order = _clientFactory.CreateClient("Api");
            PutTokenInHeaderAuthorization(token, order);

            OrderViewModel? orderUpdated;

            using var response = await order.PutAsJsonAsync($"/api/v1/orders/{model.Id}", _orderVm);
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                orderUpdated = await JsonSerializer
                    .DeserializeAsync<OrderViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }

            return orderUpdated;
        }

        public async Task<bool?> DeleteOrder(int id, string? token)
        {
            var orders = _clientFactory.CreateClient("Api");
            PutTokenInHeaderAuthorization(token, orders);

            using var response = await orders.DeleteAsync("/api/v1/orders/" + id);
        
            return response.IsSuccessStatusCode;
        }
        
        private static void PutTokenInHeaderAuthorization(string? token, HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("acess_token", token);
        }
}