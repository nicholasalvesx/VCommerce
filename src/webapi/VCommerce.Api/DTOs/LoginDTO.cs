using System.ComponentModel.DataAnnotations;

namespace VCommerce.Api.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "UserName is required")]
    public string? UserName { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "ConfirmPassword is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string? ConfirmPassword { get; set; }
}