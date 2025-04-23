using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VCommerce.Api.DTOs;
using VCommerce.Api.Models;
using VCommerce.Api.Repositores;
using VCommerce.Api.Services;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace VCommerce.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICustomerRepository _customerRepository;
    private readonly IConfiguration _configuration;

    public AuthController(ITokenService tokenService, 
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration, ICustomerRepository customerRepository)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _configuration = configuration;
        _customerRepository = customerRepository;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var userExists = await _userManager.FindByNameAsync(dto.Name!);
        if (userExists == null)
        {
            return BadRequest("User does not exist");
        }
        
        var user = await _userManager.FindByNameAsync(dto.Name!);

        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password!)) 
            return Unauthorized();
        
        var userRoles = await _userManager.GetRolesAsync(user);
            
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
            
        authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        var token = _tokenService.GenerateAcessToken(authClaims, _configuration);
            
        var refreshToken = _tokenService.GenerateRefreshToken();
            
        _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"]
            ,out var tokenExpirationInMinutes);
            
        user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(tokenExpirationInMinutes);
            
        user.RefreshToken = refreshToken;
            
        await _userManager.UpdateAsync(user);
            
        return Ok(new 
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            ExpirationToken = token.ValidTo
        });

    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] CustomerDTO customerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var cliente = new Customer(
            customerDto.Email,
            customerDto.Name,
            customerDto.Password,
            customerDto.ConfirmPassword,
            customerDto.LastName
            );
        
        await _customerRepository.CreateCustomer(cliente);
        
        var user = new ApplicationUser
        {
            Email = customerDto.Email,
            Name = customerDto.Name,
            LastName = customerDto.LastName,
            UserName = Regex.Replace(customerDto.Name!, "[^a-zA-Z0-9]", ""),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            EmailConfirmed = true
        };
        
        
        if (customerDto.Password != customerDto.ConfirmPassword)
        {
            return BadRequest(new { message = "As senhas não coincidem." });
        }

        var result = await _userManager.CreateAsync(user, customerDto.Password!);
        if (!result.Succeeded)
        {
            await _customerRepository.DeleteCustomer(customerDto.Id);
            
            return BadRequest(result.Errors);
        }
        
        return Ok(new AuthResult
        {
            Succeeded = true,
            Token = null,
            Expiration = DateTime.UtcNow.AddHours(1)
        });
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenDTO? dto)
    {
        if (dto == null)
        {
            return BadRequest("Invalid request");
        }

        var acessToken = dto.AcessToken;
        if (acessToken == null)
        {
            throw new ArgumentNullException(acessToken);
        }

        var refreshToken = dto.RefreshToken;
        if (refreshToken == null)
        {
            throw new ArgumentNullException(refreshToken);
        }

        var tokenPrincipal = _tokenService.GetPrincipalFromExpiredToken(acessToken, _configuration);
        if (tokenPrincipal == null!)
        {
            return BadRequest("Invalid access token/refresh token");
        }

        var userName = tokenPrincipal.Identity!.Name;

        var user = await _userManager.FindByNameAsync(userName!);
        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            return BadRequest("Invalid access token/refresh token");
        }

        var newAcessToken = _tokenService.GenerateAcessToken(tokenPrincipal.Claims.ToList(), _configuration);

        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            acessToken = new JwtSecurityTokenHandler().WriteToken(newAcessToken),
            refreshToken = newRefreshToken
        });
    }
    
    [HttpPost]
    [Route("revok/{userName}")]
    public async Task<IActionResult> Revoke(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            return BadRequest("User does not exist");
        }
        
        user.RefreshToken = null;
        
        await _userManager.UpdateAsync(user);
        return NoContent();
    }
} 