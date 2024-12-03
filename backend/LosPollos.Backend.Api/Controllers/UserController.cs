using LosPollos.Backend.Api.Auth;
using LosPollos.Backend.Api.Cache;
using LosPollos.Backend.Api.Context;
using LosPollos.Backend.Api.Controllers.Dtos;
using LosPollos.Backend.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LosPollos.Backend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationContext _context;
    private readonly IActivityCacheService _activityCache;

    public UsersController(ApplicationContext context, IActivityCacheService activityCache)
    {
        _context = context;
        _activityCache = activityCache;
    }

    [HttpPost("set-activity-status/{userId:int}")]
    public async Task<IActionResult> SetActivityStatusAsync([FromRoute] int userId, [FromBody] SetActivityRequest request)
    {
        User? user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        bool? isActive = await _activityCache.GetIsActiveStatusAsync(userId);
        if (isActive == null || isActive != request.IsActive)
        {
            user.IsActive = request.IsActive;
            await _context.SaveChangesAsync();
        }
        
        await _activityCache.SetIsActiveStatusAsync(userId, request.IsActive, TimeSpan.FromMinutes(30));
        return Ok();
    }
    
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
}