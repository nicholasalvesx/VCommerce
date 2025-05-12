namespace VCommerce.Mvc.Models;

public class AuthResult
{
        public int UserId { get; set; }
        public bool Succeeded { get; set; }
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }
        public string? Message { get; set; }
}