using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Services;
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
    public string GenerateToken(Guid userId, string firstName, string lastName)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, firstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, lastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var key = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:_jwtSettings.Issuer,
            audience:_jwtSettings.Audience,
            claims: claims,
            expires: _dateTimePovider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: key);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}