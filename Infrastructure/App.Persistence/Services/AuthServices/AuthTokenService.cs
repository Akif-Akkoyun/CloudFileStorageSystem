using App.Application.Interfaces;
using App.Domain.Entities;
using Duende.IdentityModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace App.Persistence.Services.AuthServices
{
    internal class AuthTokenService : IAuthTokenService
    {
        private readonly IConfiguration _configuration;

        public AuthTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(UserEntity user)
        {
            var claims = new List<Claim>
        {
            new(JwtClaimTypes.Subject, user.Id.ToString()),
            new(JwtClaimTypes.Name, user.Name),
            new(JwtClaimTypes.Email, user.Email),
            new(JwtClaimTypes.Role, user.Role?.RoleName ?? "User")
        };

            var secret = _configuration["JWT:Secret"]!;
            var issuer = _configuration["JWT:Issuer"]!;
            var expiresInMinutes = int.Parse(_configuration["JWT:ExpiresInMinutes"] ?? "30");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
