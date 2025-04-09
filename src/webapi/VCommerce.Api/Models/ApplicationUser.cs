using Microsoft.AspNetCore.Identity;

namespace VCommerce.Api.Models;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }
}