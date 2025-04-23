using VCommerce.Api.DTOs;
using VCommerce.Api.Models;

namespace VCommerce.Api.Repositores;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer?>> GetCustomers();
    
    Task<Customer?> GetCustomerById(int id);
    
    Task<Customer> CreateCustomer(Customer customer);
    
    Task<Customer> UpdateCustomer (Customer customer);
    
    Task<Customer?> DeleteCustomer(int id);
}