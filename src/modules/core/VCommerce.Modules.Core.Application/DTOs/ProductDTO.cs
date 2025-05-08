using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VCommerce.Modules.Core.Application.DTOs;

public class ProductDTO
{
    public int Id { get; set; }
    
    [Required(ErrorMessage =  "Nome do produto é obrigatorio")]
    [MinLength(3, ErrorMessage =  "Precisa de 3 caracteres no minimo")]
    [MaxLength(100, ErrorMessage =  "Nao pode exceder de 100 caracteres")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage =  "O preço do produto <UNK> é obrigatorio")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage =  "Descriçao do produto é obrigatoria")]
    [MinLength(3, ErrorMessage =  "No minimo 3 caracteres")]
    [MaxLength(500, ErrorMessage =  "Nao pode exceder de 500 caracteres")]
    public string? Description { get; set; }
    
    [Required(ErrorMessage =  "Estoque é obrigatorio")]
    [Range(1, 999)]
    public long Stock { get; set; }
    
    [JsonIgnore]
    public string? CategoryName { get; set; }
    
    public int CategoryId { get; set; }
}