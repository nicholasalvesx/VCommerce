using System.ComponentModel.DataAnnotations;
using VCommerce.Api.Models;

namespace VCommerce.Api.DTOs;

public class CategoryDTO
{
    public int CategoryId { get; set; }
    
    [Required(ErrorMessage =  "Name is required")]
    [MinLength(3, ErrorMessage =  "Name must be at least 3 characters")]
    [MaxLength(100, ErrorMessage =  "Name cannot exceed 100 characters")]
    public string? Name { get; set; }
    public ICollection<Product>? Products { get; set; }
}