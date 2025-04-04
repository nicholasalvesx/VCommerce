using System.ComponentModel.DataAnnotations;
using VCommerce.Api.Models;

namespace VCommerce.Api.DTOs;

public class ProductDTO
{
    public int Id { get; set; }
    
    [Required(ErrorMessage =  "Name is required")]
    [MinLength(3, ErrorMessage =  "Name must be at least 3 characters")]
    [MaxLength(100, ErrorMessage =  "Name cannot exceed 100 characters")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage =  "Price is required")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage =  "Description is required")]
    [MinLength(3, ErrorMessage =  "Description must be at least 3 characters")]
    [MaxLength(500, ErrorMessage =  "Description cannot exceed 500 characters")]
    public string? Description { get; set; }
    
    [Required(ErrorMessage =  "Stock is required")]
    [Range(1, 999)]
    public long Stock { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public Category? Category { get; set; }
    
    public int CategoryId { get; set; }
}