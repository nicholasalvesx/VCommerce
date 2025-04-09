using System.ComponentModel.DataAnnotations;

namespace VCommerce.Api.DTOs;

public class RegisterDTO
{
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
    
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [Required(ErrorMessage = "ConfirmPassword is required")]
    public string? ConfirmPassword { get; set; }
    
    [Required(ErrorMessage = "UserName is required")]
    public string? UserName { get; set; }
}