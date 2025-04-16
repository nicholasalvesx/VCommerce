using VCommerce.Mvc.Models;

namespace VCommerce.Mvc.Services.Contracts;

public interface ICustomerService
{
    Task<IEnumerable<CustomerViewModel>?> GetAllCustomers(string? token);
    Task<CustomerViewModel?> FindCustomerById(int id, string? token);
    Task<CustomerViewModel?> CreateCustomer(CustomerViewModel? customerVm, string? token);
    Task<CustomerViewModel?> UpdateCustomer(CustomerViewModel customerVm, string? token);
    Task<bool> DeleteCustomerById(int id, string? token);
}