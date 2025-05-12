using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VCommerce.Modules.Core.Application.DTOs;
using VCommerce.Modules.Core.Application.Interfaces;
using VCommerce.Modules.Core.Domain.Entities;
using VCommerce.Modules.Core.Infra.Models;
using VCommerce.Modules.Core.Infra.Repositories;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace VCommerce.Api.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
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
            return BadRequest("Usuario nao existe");
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
        
        if (!user.EmailConfirmed)
        {
            return Unauthorized(new { message = "É necessário confirmar seu e-mail antes de fazer login." });
        }
        
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

        if (customerDto.Password != customerDto.ConfirmPassword)
            return BadRequest(new { message = "As senhas não coincidem." });

        var cliente = new Customer(
            customerDto.Email,
            customerDto.Name,
            customerDto.Password,
            customerDto.ConfirmPassword,
            customerDto.LastName
        );

        await _customerRepository.CreateCustomer(cliente);
        
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
            return BadRequest("Token nao existe");
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
            return BadRequest("Token invalido");
        }

        var userName = tokenPrincipal.Identity!.Name;

        var user = await _userManager.FindByNameAsync(userName!);
        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
        {
            return BadRequest("Token invalido");
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
            return BadRequest("Usuario nao econtrado");
        }
        
        user.RefreshToken = null;
        
        await _userManager.UpdateAsync(user);
        return NoContent();
    }
    
    [HttpGet]
    [Route("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
        if (userId == null || code == null)
            return BadRequest(new { message = "Parâmetros inválidos." });

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound(new { message = "Usuário não encontrado." });

        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            return Ok("Email confirmado, volte ao site!");
        }
        
        return BadRequest(new { message = "Erro ao enviar email." });
    }

} 