using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Controllers;

[Authorize]
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
    
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        
        if (!userId.HasValue)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var id))
            {
                userId = id;
            }
        }
    
        if (!userId.HasValue)
        {
            return RedirectToAction("Login", "Account");
        }
    
        var token = HttpContext.Session.GetString("JWTToken");
    
        if (string.IsNullOrEmpty(token))
        {
            token = await HttpContext.GetTokenAsync("access_token");
        }
    
        var customer = await _customerService.FindCustomerById(userId.Value, token);
    
        if (customer == null)
        {
            return RedirectToAction("Login", "Account");
        }
    
        return View(customer);
    }
    
    private async Task<string?> GetAccessToken()
    {
        return await HttpContext.GetTokenAsync("access_token");
    }
}