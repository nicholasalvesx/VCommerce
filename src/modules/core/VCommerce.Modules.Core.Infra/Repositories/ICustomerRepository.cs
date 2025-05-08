using VCommerce.Modules.Core.Domain.Entities;

namespace VCommerce.Modules.Core.Infra.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer?>> GetCustomers();
    
    Task<Customer?> GetCustomerById(int id);
    
    Task<Customer> CreateCustomer(Customer customer);
    
    Task<Customer> UpdateCustomer (Customer customer);
    
    Task<Customer?> DeleteCustomer(int id);
}