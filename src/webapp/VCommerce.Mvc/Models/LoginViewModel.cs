using System.ComponentModel.DataAnnotations;

namespace VCommerce.Mvc.Models;

public class LoginViewModel
{
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
        
        public string? ReturnUrl { get; set; }
    
}