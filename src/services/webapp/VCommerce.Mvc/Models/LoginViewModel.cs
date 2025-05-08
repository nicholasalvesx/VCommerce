using System.ComponentModel.DataAnnotations;

namespace VCommerce.Mvc.Models;

public class LoginViewModel
{
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
        
        public string? ReturnUrl { get; set; }
    
}