using VCommerce.Modules.Core.Application.DTOs;

namespace VCommerce.Modules.Core.Application.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderDTO>> GetOrder();
    Task<IEnumerable<OrderDTO>> GetOrderCustomer();
    Task<OrderDTO> GetOrderById(int id);
    Task AddOrder(OrderDTO order);
    Task UpdateOrder(OrderDTO order);
    Task DeleteOrder(int id);   
}