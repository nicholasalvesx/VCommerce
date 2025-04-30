using System.ComponentModel.DataAnnotations;
using VCommerce.Api.Models;

namespace VCommerce.Api.DTOs;

public class CategoryDTO
{
    public int CategoryId { get; set; }
    
    [Required(ErrorMessage =  "O nome é obrigatorio")]
    [MinLength(3, ErrorMessage =  "Precisa de 3 caracteres no mininmo")]
    [MaxLength(100, ErrorMessage =  "Nao pode exceder de 100 caracteres")]
    public string? CategoryName { get; set; }
    public ICollection<Product>? Products { get; set; }
}