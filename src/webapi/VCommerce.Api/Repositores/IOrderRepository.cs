using VCommerce.Api.Models;

namespace VCommerce.Api.Repositores;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrders();
    
    Task<IEnumerable<Order>> GetOrdersCustomers();
    
    Task<Order?> GetOrderById(int id);
    
    Task<Order> CreateOrder(Order order);
    
    Task<Order> UpdateOrder(Order order);
    
    Task<Order> DeleteOrder(int id);
}