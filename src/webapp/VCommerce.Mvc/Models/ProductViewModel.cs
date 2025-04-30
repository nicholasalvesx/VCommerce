using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VCommerce.Mvc.Models;

public class ProductViewModel
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Description { get; set; }

    [Required]
    [Range(1,9999)]
    public decimal Price { get; set; }

    [Required]
    [Range(1,9999)]
    public long Stock { get; set; }
    [Display(Name = "Nome da categoria")]
    public CategoryViewModel? CategoryName { get; set; }

    [Range(1, 100)]
    public int Quantity { get; set; } = 1;
    
    [JsonIgnore]
    [Display(Name="CategoriaId")]
    public int CategoryId { get; set; }
}
