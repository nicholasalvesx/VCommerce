using Microsoft.AspNetCore.Mvc;
using VCommerce.Modules.Core.Application.DTOs;
using VCommerce.Modules.Core.Application.Interfaces;

namespace VCommerce.Api.Controllers;

[ApiController]
[Route("api/v1/customers")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await _customerService.GetCustomers();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var customer = await _customerService.GetCustomerById(id);
        return Ok(customer);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreteCustomer([FromBody] CustomerDTO? customerDto)
    {
        if (customerDto == null)
            return BadRequest("Cliente nao existe");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _customerService.AddCustomer(customerDto);
        return Ok(customerDto);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, CustomerDTO customerDto)
    {
        if (id != customerDto.Id)
        {
            return BadRequest();
        }
        
        await _customerService.UpdateCustomer(customerDto);
        return Ok(customerDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await _customerService.GetCustomerById(id);
        if (customer == null!)
        {
            return NotFound("Cliente nao existe");
        } 
        await _customerService.DeleteCustomer(id);
        return Ok(customer);
    }
}