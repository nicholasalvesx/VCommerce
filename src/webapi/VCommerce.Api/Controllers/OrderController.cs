using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VCommerce.Api.DTOs;
using VCommerce.Api.Services;

namespace VCommerce.Api.Controllers;

[ApiController]
[Route("api/v1/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _orderService.GetOrder();
        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderService.GetOrderById(id);
        return Ok(order);
    }

    [HttpGet("customers")]
    public async Task<IActionResult> GetOrdersByCustomer()
    {
        var orders = await _orderService.GetOrderCustomer();
        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDTO? order)
    {
        if (order == null)
            return BadRequest();
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        await _orderService.AddOrder(order);
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateOrder(int id, OrderDTO? order)
    {
        Debug.Assert(order != null, nameof(order) + " != null");
        
        if (id != order.Id)
        {
            return BadRequest("Id do pedido invalido");
        }
        
        await _orderService.UpdateOrder(order);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOrder(int id, OrderDTO? order)
    {
        if (order != null && id != order.Id)
            return NotFound("id do pedido invalido");
        
        await _orderService.GetOrderById(id);
        
        await _orderService.DeleteOrder(id);
        return Ok();
    }
}