using System.ComponentModel.DataAnnotations;

namespace VCommerce.Mvc.Models;

public class CategoryViewModel
{
    public int CategoryId { get; set; }
    [Required]
    public string? Name { get; set; }
}
