using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PalworldServers.Grpc.Services.Interfaces;

namespace PalworldServers.Grpc.Services.Tokens;

public sealed record TokenService(string SysKey) : ITokenService
{
    private static readonly TimeSpan TokenLifeTime = TimeSpan.FromHours(2);

    public string GenerateJwtToken(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(JwtRegisteredClaimNames.Email, email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SysKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDesc = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TokenLifeTime),
            Issuer = "https://id.palworld-servers.com",
            Audience = "https://grpc.palworld-servers.com",
            SigningCredentials = creds
        };

        var token = tokenHandler.CreateToken(tokenDesc);
        return tokenHandler.WriteToken(token);
    }
}