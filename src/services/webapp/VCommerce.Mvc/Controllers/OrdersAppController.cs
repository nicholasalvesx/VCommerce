using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VCommerce.Mvc.Models;
using VCommerce.Mvc.Services.Contracts;

namespace VCommerce.Mvc.Controllers;

[Authorize]
public class OrdersAppController : Controller
{
    private readonly IOrderService _orderService;

    public OrdersAppController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var orders = await _orderService.GetAllOrders(await GetAccessToken());
        return View(orders);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderViewModel>>> Details(int id)
    {
        var order = await _orderService.GetOrderById(id, await GetAccessToken());
            
        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    [HttpGet]
    public IActionResult CreateOrder()
    {
        var order = new OrderViewModel
        {
            OrderDate = DateTime.Now,
            Status = "Pendente",
            OrderItems = new List<OrderItemViewModel>(),
            OrderHistory = new List<OrderHistoryViewModel>(),
            ShippingAddress = new AddressViewModel(),
            BillingAddress = new AddressViewModel()
        };

        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOrder(OrderViewModel order)
    {
        if (ModelState.IsValid)
        {
            order.OrderNumber = "ORD-" + DateTime.Now.ToString("yyyyMMdd") + "-" + new Random().Next(1000, 9999);
                
            order.OrderHistory.Add(new OrderHistoryViewModel
            {
                Date = DateTime.Now,
                Status = order.Status,
                Description = "Pedido criado"
            });
                
            var result = await _orderService.CreateOrder(order, await GetAccessToken());
                
            if (result != null)
            {
                return RedirectToAction(nameof(Index));
            }
                
            ModelState.AddModelError("", "Erro ao criar o pedido. Por favor, tente novamente.");
        }
            
        return View(order);
    }
        
    [HttpGet]
    public async Task<IActionResult> EditOrder(int id)
    {
        var order = await _orderService.GetOrderById(id, await GetAccessToken());
            
        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditOrder(int id, OrderViewModel order)
    {
        if (id != order.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid) 
            return View(order);
        
        order.OrderHistory.Add(new OrderHistoryViewModel
        {
            Date = DateTime.Now,
            Status = order.Status,
            Description = "Pedido atualizado"
        });
                
        var result = await _orderService.UpdateOrder(order, await GetAccessToken());
                
        if (result != null)
        {
            return RedirectToAction(nameof(Details), new { id = order.Id });
        }
                
        ModelState.AddModelError("", "Erro ao atualizar o pedido. Por favor, tente novamente.");

        return View(order);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _orderService.GetOrderById(id, await GetAccessToken());
            
        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    [HttpDelete, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _orderService.DeleteOrder(id, await GetAccessToken());
            
        if ((bool)(!result)!)
        {
            return NotFound();
        }
            
        return RedirectToAction(nameof(Index));
    }
        
    [HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        var order = await _orderService.GetOrderById(id, await GetAccessToken());
            
        if (order == null)
        {
            return NotFound();
        }
            
        order.Status = status;
            
        order.OrderHistory.Add(new OrderHistoryViewModel
        {
            Date = DateTime.Now,
            Status = status,
            Description = $"Status atualizado para {status}"
        });
            
        var result = await _orderService.UpdateOrder(order, await GetAccessToken());
            
        if (result != null)
        {
            return RedirectToAction(nameof(Details), new { id = order.Id });
        }
            
        return BadRequest("Erro ao atualizar o status do pedido.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddNote(int id, string note)
    {
        if (string.IsNullOrWhiteSpace(note))
        {
            return BadRequest("A nota não pode estar vazia.");
        }
            
        var order = await _orderService.GetOrderById(id, await GetAccessToken());
            
        if (order == null)
        {
            return NotFound();
        }
            
        order.Notes = string.IsNullOrEmpty(order.Notes) 
            ? note 
            : $"{order.Notes}\n\n{DateTime.Now:dd/MM/yyyy HH:mm} - {note}";
            
        var result = await _orderService.UpdateOrder(order, await GetAccessToken());
            
        if (result != null)
        {
            return RedirectToAction(nameof(Details), new { id = order.Id });
        }
            
        return BadRequest("Erro ao adicionar nota ao pedido.");
    }

    private async Task<string?> GetAccessToken()
    {
        return await HttpContext.GetTokenAsync("acess_token");
    }
}