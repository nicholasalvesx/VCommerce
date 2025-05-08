using System.ComponentModel.DataAnnotations;

namespace VCommerce.Modules.Core.Application.DTOs;

public class CustomerDTO
{
    [Key]
    public int Id { get; set; }
    
    [EmailAddress]
    [Required(ErrorMessage = "Email é obrigatorio")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage =  "O nome é obrigatorio")]
    [MinLength(3, ErrorMessage =  "Precisa de no minimo 3 caracteres")]
    [MaxLength(100, ErrorMessage =  "Precisa de 100 caracteres no maximo")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Sobrenome é obrigatorio")]
    [MinLength(3, ErrorMessage =  "Precisa de no minimo 3 caracteres")]
    [MaxLength(100, ErrorMessage =  "Precisa de 100 caracteres no maximo")]
    public string? LastName { get; set; }
    
    [Required(ErrorMessage = "Senha é obrigatoria")]
    public string? Password { get; set; }
    
    [Compare("Password", ErrorMessage = "As senhas nao conicidem")]
    [Required(ErrorMessage = "Confirme a senha")]
    public string? ConfirmPassword { get; set; }

    public bool IsAdm { get; set; }
    
}