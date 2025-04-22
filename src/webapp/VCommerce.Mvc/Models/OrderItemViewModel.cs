using System.ComponentModel.DataAnnotations;

namespace VCommerce.Mvc.Models;

public class OrderItemViewModel
{
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    public string? ProductName { get; set; } // para exibição

    [Required]
    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public int OrderId { get; set; }
}