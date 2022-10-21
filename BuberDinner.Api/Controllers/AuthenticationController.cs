using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var result = _authenticationService.Register(request.FirsName, request.LastName, request.Email, request.Password);
        var response = new AuthenticationResponse(
            result.User.Id,
            result.User.FirstName,
            result.User.LastName,
            result.User.Email,
            result.Token
        );
        return Ok(response);
    }
    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var login = _authenticationService.Login(request.Email, request.Password);
        var response = new AuthenticationResponse(
            login.User.Id,
            login.User.FirstName,
            login.User.LastName,
            login.User.Email,
            login.Token
        );
        return Ok(response);
    }
}