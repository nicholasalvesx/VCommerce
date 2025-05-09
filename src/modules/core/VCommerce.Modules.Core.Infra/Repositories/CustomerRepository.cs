using Microsoft.EntityFrameworkCore;
using VCommerce.Modules.Core.Domain.Entities;

namespace VCommerce.Modules.Core.Infra.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer?>> GetCustomers()
    {
        return await _context.Customers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Customer?> GetCustomerById(int id)
    { 
        return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer> CreateCustomer(Customer customer)
    {
        await _context.AddAsync(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer> UpdateCustomer(Customer customer)
    {
        _context.Entry(customer).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer?> DeleteCustomer(int id)
    {
        var customer = await GetCustomerById(id);
        _context.Customers.Remove(customer ?? throw new InvalidOperationException());
        await _context.SaveChangesAsync();
        return customer;
    }
}