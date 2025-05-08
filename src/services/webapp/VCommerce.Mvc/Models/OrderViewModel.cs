using System.ComponentModel.DataAnnotations;

namespace VCommerce.Mvc.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        
        [Display(Name = "Número do Pedido")]
        public string? OrderNumber { get; set; }
        
        [Display(Name = "Data do Pedido")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }
        
        [Display(Name = "Nome do Cliente")]
        public string? CustomerName { get; set; }
        
        [Display(Name = "Email do Cliente")]
        [EmailAddress]
        public string? CustomerEmail { get; set; }
        
        [Display(Name = "Telefone do Cliente")]
        public string? CustomerPhone { get; set; }
        
        [Display(Name = "Status")]
        public string? Status { get; set; }
        
        [Display(Name = "Valor Total")]
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }
        
        [Display(Name = "Valor do Frete")]
        [DataType(DataType.Currency)]
        public decimal ShippingAmount { get; set; }
        
        [Display(Name = "Valor do Desconto")]
        [DataType(DataType.Currency)]
        public decimal DiscountAmount { get; set; }
        
        [Display(Name = "Método de Pagamento")]
        public string? PaymentMethod { get; set; }
        
        [Display(Name = "Status do Pagamento")]
        public string? PaymentStatus { get; set; }
        
        [Display(Name = "Detalhes do Pagamento")]
        public string? PaymentDetails { get; set; }
        
        [Display(Name = "Observações")]
        public string? Notes { get; set; }
        
        [Display(Name = "Endereço de Entrega")]
        public AddressViewModel ShippingAddress { get; set; }
        
        [Display(Name = "Endereço de Cobrança")]
        public AddressViewModel BillingAddress { get; set; }
        
        [Display(Name = "Itens do Pedido")]
        public List<OrderItemViewModel> OrderItems { get; set; }
        
        [Display(Name = "Histórico do Pedido")]
        public List<OrderHistoryViewModel> OrderHistory { get; set; }
    }
    
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        
        public int ProductId { get; set; }
        
        [Display(Name = "Produto")]
        public string? ProductName { get; set; }
        
        [Display(Name = "SKU")]
        public string? ProductSku { get; set; }
        
        [Display(Name = "Imagem")]
        public string? ProductImageUrl { get; set; }
        
        [Display(Name = "Preço Unitário")]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }
        
        [Display(Name = "Quantidade")]
        public int Quantity { get; set; }
    }
    
    public class OrderHistoryViewModel
    {
        [Display(Name = "Data")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        
        [Display(Name = "Status")]
        public string? Status { get; set; }
        
        [Display(Name = "Descrição")]
        public string? Description { get; set; }
    }
    
    public class AddressViewModel
    {
        [Display(Name = "Nome do Destinatário")]
        public string? RecipientName { get; set; }
        
        [Display(Name = "Rua")]
        public string? Street { get; set; }
        
        [Display(Name = "Número")]
        public string? Number { get; set; }
        
        [Display(Name = "Complemento")]
        public string? Complement { get; set; }
        
        [Display(Name = "Bairro")]
        public string? Neighborhood { get; set; }
        
        [Display(Name = "Cidade")]
        public string? City { get; set; }
        
        [Display(Name = "Estado")]
        public string? State { get; set; }
        
        [Display(Name = "CEP")]
        public string? ZipCode { get; set; }
    }
}
