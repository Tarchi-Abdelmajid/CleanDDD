using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Common.Services;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDateTimePovider _dateTimePovider;
    private  readonly IUserRepository _userRepository;
    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IDateTimePovider dateTimePovider,
        IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _dateTimePovider = dateTimePovider;
        _userRepository = userRepository;
    }

    public AuthenticationResult Register(string firstName, string lastName, string email, string password)
    {
        // check if user exists
        if(_userRepository.GetUserByeMail(email) is not null)
            throw new Exception("User already exists");
        // if not, create user
        var user = new User{FirstName = firstName, LastName = lastName, Email = email, Password = password};
        _userRepository.Add(user);
        // Create token
        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(
            user,
            token);
    }

    public AuthenticationResult Login(string email, string password)
    {
        if(_userRepository.GetUserByeMail(email) is not  User user)
            throw new Exception("User does not exist");
        if(user.Password != password)
            throw new Exception("Password is incorrect");
        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(
            user,
            token);
    }
}