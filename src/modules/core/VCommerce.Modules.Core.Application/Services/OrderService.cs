using AutoMapper;
using VCommerce.Modules.Core.Application.DTOs;
using VCommerce.Modules.Core.Application.Interfaces;
using VCommerce.Modules.Core.Domain.Entities;
using VCommerce.Modules.Core.Infra.Repositories;

namespace VCommerce.Modules.Core.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderDTO>> GetOrder()
    {
        var orderEntites = await _orderRepository.GetOrders();
        return _mapper.Map<IEnumerable<OrderDTO>>(orderEntites);
    }

    public async Task<IEnumerable<OrderDTO>> GetOrderCustomer()
    {
        var orderEntites = await _orderRepository.GetOrdersCustomers();
        return _mapper.Map<IEnumerable<OrderDTO>>(orderEntites);
    }

    public async Task<OrderDTO> GetOrderById(int id)
    {
        var orderEntites = await _orderRepository.GetOrderById(id);
        return _mapper.Map<OrderDTO>(orderEntites);
    }

    public async Task AddOrder(OrderDTO order)
    {
        var orderEntity = _mapper.Map<Order>(order);
        await _orderRepository.CreateOrder(orderEntity);
        order.Id = orderEntity.Id;
    }

    public async Task UpdateOrder(OrderDTO order)
    {
        var orderEntity = _mapper.Map<Order>(order);
        await _orderRepository.UpdateOrder(orderEntity);
    }

    public async Task DeleteOrder(int id)
    {
        var orderEntity = _orderRepository.GetOrderById(id).Result;
        
        if (orderEntity != null) 
            await _orderRepository.DeleteOrder(orderEntity.Id);
    }
}