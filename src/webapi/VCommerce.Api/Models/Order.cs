using System.Text.Json.Serialization;
using VCommerce.Api.DTOs;

namespace VCommerce.Api.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal Amount { get; set; }
    public string? Status { get; set; }
    public Customer? Customer { get; set; }
    public int CustomerId { get; set; }
    
    [JsonIgnore]
    public IList<OrderItemDTO>? Items { get; set; }
}