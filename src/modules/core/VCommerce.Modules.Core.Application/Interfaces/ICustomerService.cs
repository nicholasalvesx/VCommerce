using VCommerce.Modules.Core.Application.DTOs;

namespace VCommerce.Modules.Core.Application.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDTO>> GetCustomers();
    Task<CustomerDTO> GetCustomerById(int id);
    Task AddCustomer(CustomerDTO? customer);
    Task UpdateCustomer(CustomerDTO customer);
    Task DeleteCustomer(int id);
}