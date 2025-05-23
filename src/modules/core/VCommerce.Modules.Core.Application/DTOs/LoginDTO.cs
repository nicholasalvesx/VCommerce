using System.ComponentModel.DataAnnotations;

namespace VCommerce.Modules.Core.Application.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "O nome é obrigatorio")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "A senha é obrigatoria")]
    public string? Password { get; set; }

}