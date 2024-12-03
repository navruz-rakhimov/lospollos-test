using System.Security.Claims;
using LosPollos.Backend.Api.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LosPollos.Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    [Authorize(Policy = AuthPolicy.ActiveUser)]
    [HttpGet("welcome")]
    public IActionResult Welcome()
    {
        string username = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        return Ok(new
        {
            WelcomeMessage = $"Welcome {username}. This page is for authenticated users only!"
        });
    }
}