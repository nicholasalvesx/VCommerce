using System.ComponentModel.DataAnnotations;

namespace VCommerce.Mvc.Models;

public class OrderViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "CustomerId is required")]
    public int CustomerId { get; set; }

    public string? CustomerName { get; set; } // caso queira exibir na view

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "Total amount is required")]
    public decimal TotalAmount { get; set; }

    public List<OrderItemViewModel>? Items { get; set; }
}