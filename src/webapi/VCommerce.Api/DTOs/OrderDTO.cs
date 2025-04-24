using System.ComponentModel.DataAnnotations;
using VCommerce.Api.Models;

namespace VCommerce.Api.DTOs;

public class OrderDTO
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Id do cliente <UNK> obrigatorio")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Data do pedido é obrigatoria")]
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "O total da conta é necessario")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Total precisa ser maior do que 0")]
    public decimal TotalAmount { get; set; }

    public List<OrderItem>? Items { get; set; }
}