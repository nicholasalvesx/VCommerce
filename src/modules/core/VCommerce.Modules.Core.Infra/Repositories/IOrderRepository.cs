using VCommerce.Modules.Core.Domain.Entities;

namespace VCommerce.Modules.Core.Infra.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrders();
    
    Task<IEnumerable<Order>> GetOrdersCustomers();
    
    Task<Order?> GetOrderById(int id);
    
    Task<Order> CreateOrder(Order order);
    
    Task<Order> UpdateOrder(Order order);
    
    Task<Order> DeleteOrder(int id);
}