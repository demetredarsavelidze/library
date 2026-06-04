using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryManagement.Application.Abstractions.Authentication;
using LibraryManagement.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagement.Infrastructure.Security;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public (string Token, DateTime ExpiresAtUtc) GenerateToken(ApplicationUser user)
    {
        var key = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key configuration is required.");
        var issuer = _configuration["Jwt:Issuer"] ?? "LibraryManagement";
        var audience = _configuration["Jwt:Audience"] ?? "LibraryManagement";
        var expiresMinutes = int.TryParse(_configuration["Jwt:ExpiresMinutes"], out var value) ? value : 60;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expiresAtUtc = DateTime.UtcNow.AddMinutes(expiresMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: expiresAtUtc,
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAtUtc);
    }
}
