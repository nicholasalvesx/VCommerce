using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using VCommerce.Api.Data;
using VCommerce.Api.Models;

namespace VCommerce.Api.Repositores;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetOrders()
    {
        return await _context.Orders.
            AsNoTracking().
            ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersCustomers()
    {
        return await _context.Orders.Include(c => c.Customer)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderById(int id)
    {
        return await _context.Orders.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Order> CreateOrder(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> UpdateOrder(Order order)
    {
        _context.Entry(order).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> DeleteOrder(int id)
    {
        var order = await GetOrderById(id);
        _context.Orders.Remove(order!);
        await _context.SaveChangesAsync();
        return order!;
    }
}