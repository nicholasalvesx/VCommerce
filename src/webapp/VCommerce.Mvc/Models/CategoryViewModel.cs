using System.ComponentModel.DataAnnotations;
using VCommerce.Api.Models;

namespace VCommerce.Mvc.Models;

public class CategoryViewModel
{
    public int CategoryId { get; set; }
    
    [Required(ErrorMessage = "O nome da categoria é obrigatório")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo {1} caracteres")]
    [Display(Name = "Nome")]
    public string? Name { get; set; }
    
    public ICollection<Product>? Products { get; set; }
}
