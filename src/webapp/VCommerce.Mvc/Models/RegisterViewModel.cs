using System.ComponentModel.DataAnnotations;

namespace VCommerce.Mvc.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(50, ErrorMessage = "O nome deve ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "O sobrenome é obrigatório")]
    [StringLength(50, ErrorMessage = "O sobrenome deve ter entre {2} e {1} caracteres", MinimumLength = 2)]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória")]
    [StringLength(100, ErrorMessage = "A senha deve ter pelo menos {2} caracteres", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "As senhas não conferem")]
    public string? ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Você deve aceitar os termos para continuar")]
    public bool AcceptTerms { get; set; }
        
    public string? ReturnUrl { get; set; }
}