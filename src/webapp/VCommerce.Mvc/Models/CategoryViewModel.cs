using System.ComponentModel.DataAnnotations;

namespace VCommerce.Mvc.Models
{
    public class CategoryViewModel
    {
        [Key]
        public int CategoryId { get; set; }
        
        [Required(ErrorMessage = "O nome da categoria é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo {1} caracteres")]
        [Display(Name = "Nome")]
        public string? Name { get; set; }
        
        [Display(Name = "Descrição")]
        public string? Description { get; set; }
        
        [Display(Name = "Ativo")]
        public bool IsActive { get; set; } = true;
        
        [Display(Name = "Ícone")]
        public string? IconClass { get; set; }
        
        public int ProductCount { get; set; }
    }
}