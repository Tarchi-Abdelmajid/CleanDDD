using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Services;
using BuberDinner.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace BuberDinner.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private  readonly IDateTimePovider _dateTimePovider;
    private readonly JwtSettings _jwtSettings;
    public JwtTokenGenerator(IDateTimePovider dateTimePovider, IOptions<JwtSettings> jwtOptions)
    {
        _dateTimePovider = dateTimePovider;
        _jwtSettings = jwtOptions.Value;
    }
    public string GenerateToken(User user)
    {
        var key = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        
        var token = new JwtSecurityToken(
            issuer:_jwtSettings.Issuer,
            audience:_jwtSettings.Audience,
            claims: claims,
            expires: _dateTimePovider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: key);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}