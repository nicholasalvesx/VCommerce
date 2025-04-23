namespace VCommerce.Api.Models;

public class AuthResult
{
    public bool Succeeded { get; set; }
    public string? Token { get; set; }
    public DateTime Expiration { get; set; }
    
    public List<string>? Errors { get; set; }
}