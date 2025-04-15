using VCommerce.Api.DTOs;

namespace VCommerce.Api.Services;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDTO>> GetCustomers();
    Task<CustomerDTO> GetCustomerById(int id);
    Task AddCustomer(CustomerDTO customer);
    Task UpdateCustomer(CustomerDTO customer);
    Task DeleteCustomer(int id);
}