using System.ComponentModel.DataAnnotations;
using VCommerce.Api.Models;

namespace VCommerce.Api.DTOs;

public class OrderDTO
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "CustomerId is required")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Order date is required")]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "Total amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Total must be greater than zero")]
    public decimal TotalAmount { get; set; }

    public List<OrderItem>? Items { get; set; }
}