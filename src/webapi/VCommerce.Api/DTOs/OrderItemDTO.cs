using System.ComponentModel.DataAnnotations;

namespace VCommerce.Api.DTOs;

public class OrderItemDTO
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }

    public int OrderId { get; set; }
}