using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VCommerce.Modules.Core.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal Amount { get; set; }
    public string? Status { get; set; }
    public Customer? Customer { get; set; }
    public int CustomerId { get; set; }
    
    [JsonIgnore]
    public IList<OrderItem>? Items { get; set; }
}

public class OrderItem
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