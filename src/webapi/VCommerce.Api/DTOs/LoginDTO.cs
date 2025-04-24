using System.ComponentModel.DataAnnotations;

namespace VCommerce.Api.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "O nome é obrigatorio")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "A senha é obrigatoria")]
    public string? Password { get; set; }

}