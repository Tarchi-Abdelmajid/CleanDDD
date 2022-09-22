using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Services;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDateTimePovider _dateTimePovider;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IDateTimePovider dateTimePovider)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _dateTimePovider = dateTimePovider;
    }

    public AuthenticationResult Register(string firstName, string lastName, string email, string password)
    {
        // check if user exists
        
        // if not, create user
        
        // Create token
        var userId = Guid.NewGuid();
        var token = _jwtTokenGenerator.GenerateToken(userId, firstName, lastName);
        return new AuthenticationResult(
            userId,
            firstName,
            lastName,
            email,
            token);
    }

    public AuthenticationResult Login(string email, string password)
    {
        return new AuthenticationResult(
            Guid.NewGuid(),
            "firstName",
            "lastName",
            email,
            "token");
    }
}