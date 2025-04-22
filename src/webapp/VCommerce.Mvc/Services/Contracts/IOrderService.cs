using VCommerce.Mvc.Models;

namespace VCommerce.Mvc.Services.Contracts;

public interface IOrderService
{
    Task<IEnumerable<OrderViewModel>?> GetAllOrders(string? token);
    Task<OrderViewModel?> GetOrderById(int id, string? token);
    Task<OrderViewModel?> CreateOrder(OrderViewModel model, string accessToken);
    Task<OrderViewModel?> UpdateOrder(OrderViewModel model, string? token);
    Task<bool?> DeleteOrder(int id, string? token);
}