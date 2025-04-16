using System.ComponentModel.DataAnnotations;

namespace VCommerce.Api.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "UserName is required")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

}