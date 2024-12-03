using LosPollos.Backend.Api.Auth;
using LosPollos.Backend.Api.Context;
using LosPollos.Backend.Api.Controllers.Dtos;
using LosPollos.Backend.Api.Entities;
using LosPollos.Backend.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LosPollos.Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthController(IUserService userService, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userService = userService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        User? user = await _userService.AuthenticateAsync(request.Username, request.Password);
        if (user is not { IsActive: true })
        {
            return Unauthorized("We could not log you in. Please check your username/password and try again.");
        }

        string token = _jwtTokenGenerator.GenerateToken(user);
        return Ok(new
        {
            user.Id,
            user.IsActive,
            user.Username,
            Token = token
        });
    }
}
