namespace VCommerce.Modules.Core.Domain.Entities;

public class AuthResult
{
    public bool Succeeded { get; set; }
    public string? Token { get; set; }
    public DateTime Expiration { get; set; }
}