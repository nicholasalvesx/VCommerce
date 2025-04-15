using System.ComponentModel.DataAnnotations;

namespace VCommerce.Api.DTOs;

public class CustomerDTO
{
    [Key]
    public int Id { get; set; }
    
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage =  "Name is required")]
    [MinLength(3, ErrorMessage =  "Name must be at least 3 characters")]
    [MaxLength(100, ErrorMessage =  "Name cannot exceed 100 characters")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
    
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [Required(ErrorMessage = "ConfirmPassword is required")]
    public string? ConfirmPassword { get; set; }
    
}