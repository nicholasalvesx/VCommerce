using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Controllers;

public class CustomerAppController : Controller
{
    private readonly ICustomerService _customerService;

    public CustomerAppController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerViewModel>>> Index()
    {
        var result = await _customerService.GetAllCustomers(await GetAccessToken());
        
        return result is null ? View("Error") : View(result);
    }

    [HttpGet]
    public Task<ActionResult<IEnumerable<CustomerViewModel>>> CreateCustomer()
    {
        return Task.FromResult<ActionResult<IEnumerable<CustomerViewModel>>>(View());
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<CustomerViewModel>>> CreateCustomer(CustomerViewModel? model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _customerService.CreateCustomer(model);
        
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public Task<ActionResult<IEnumerable<CustomerViewModel>>> EditCustomer(int id)
    {
        return Task.FromResult<ActionResult<IEnumerable<CustomerViewModel>>>(View());
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<CustomerViewModel>>> EditCustomer(CustomerViewModel? model)
    {
        if (!ModelState.IsValid) 
            return View(model);
        
        var result = await _customerService.UpdateCustomer(model, await GetAccessToken());

        if (result == null)
            return RedirectToAction(nameof(Index));
        
        return View(model);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerViewModel>>> DeleteCustomer(int id)
    {
        var result = await _customerService.FindCustomerById(id, await GetAccessToken());
        return View(result);
    }

    [HttpPost, ActionName("DeleteCustomer")]
    public async Task<ActionResult<IEnumerable<CustomerViewModel>>> DeleteCustomerConfirmed(int id)
    {
        var result = await _customerService.DeleteCustomerById(id, await GetAccessToken());

        if (!result)
            return View("Error");

        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Profile(int id)
    {
        var profile = await _customerService.FindCustomerById(id, await GetAccessToken());

        if (profile is null)
        {
            return NotFound("Any user has not been logged in");
        }
        
        return View();
    }   
    
    private async Task<string?> GetAccessToken()
    {
        return await HttpContext.GetTokenAsync("access_token");
    }
}