using System.Text.Json.Serialization;

namespace VCommerce.Api.Models;

public class Category
{
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    
    [JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}