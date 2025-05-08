using System.Text.Json.Serialization;

namespace VCommerce.Modules.Core.Domain.Entities;

public class Category
{
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    
    [JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}