namespace EmployeeManagementAPI.Services;

using EmployeeManagementAPI.Entities;
using EmployeeManagementAPI.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _secretKey;

    public TokenService(IConfiguration config)
    {
        _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["SecretKey"]));
    }

    public string GenerateToken(User user)
    {
        // var tokenHandler = new JwtSecurityTokenHandler();
        // var key = Encoding.ASCII.GetBytes(_secretKey);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Username)
        };

        var creds = new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha512Signature);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddHours(1),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}