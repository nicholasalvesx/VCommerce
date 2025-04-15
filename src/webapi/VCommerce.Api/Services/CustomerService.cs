using AutoMapper;
using VCommerce.Api.DTOs;
using VCommerce.Api.Models;
using VCommerce.Api.Repositores;

namespace VCommerce.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDTO>> GetCustomers()
    {
        var customers = await _customerRepository.GetCustomers();
        return _mapper.Map<IEnumerable<CustomerDTO>>(customers);
    }

    public async Task<CustomerDTO> GetCustomerById(int id)
    {
        var customer = await _customerRepository.GetCustomerById(id);
        return _mapper.Map<CustomerDTO>(customer);
    }

    public async Task AddCustomer(CustomerDTO customer)
    {
        var customerEntity = _mapper.Map<Customer>(customer);
        await _customerRepository.CreateCustomer(customerEntity);
        customer.Id = customerEntity.Id; 
    }

    public async Task UpdateCustomer(CustomerDTO customer)
    {
        var customerEntity = _mapper.Map<Customer>(customer);
        await _customerRepository.UpdateCustomer(customerEntity);
    }

    public async Task DeleteCustomer(int id)
    {
        var customer = await _customerRepository.GetCustomerById(id);
        if (customer != null) await _customerRepository.DeleteCustomer(customer.Id);
    }
}