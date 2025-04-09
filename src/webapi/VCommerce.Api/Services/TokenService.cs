using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace VCommerce.Api.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public JwtSecurityToken GenerateAcessToken(IEnumerable<Claim> claims, IConfiguration configuration)
    {
        var key = _configuration.GetSection("JWT").GetValue<string>("SecretKey");
        if (key == null)
        {
            throw new InvalidOperationException("JWT secret key is invalid");
        }
        
        var privateKey = Encoding.UTF8.GetBytes(key);
        
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey),
            SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            
            Expires = DateTime.UtcNow
                .AddMinutes(configuration.GetSection("JWT")
                    .GetValue<double>("TokenValidityInMinutes")),
            
            Audience = _configuration.GetSection("JWT")
                .GetValue<string>("ValidAudience"),  
            
            Issuer = _configuration.GetSection("JWT")
                .GetValue<string>("ValidIssuer"),
            
            SigningCredentials = signingCredentials
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        return token;
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[128];

        using var randomNumberGenerator = RandomNumberGenerator.Create();
        
        randomNumberGenerator.GetBytes(randomBytes);
        
        var refreshToken = Convert.ToBase64String(randomBytes);
        return refreshToken;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration configuration)
    {
        var secretKey = _configuration.GetSection("JWT:SecretKey").GetValue<string>("SecretKey");
        if (secretKey == null)
        {
            throw new InvalidOperationException("JWT secret key is invalid");
        }

        var tokenValidateParams = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var tokenPrincipal  = tokenHandler.ValidateToken(token, tokenValidateParams, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        
        return tokenPrincipal;
    }
}