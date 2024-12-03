using System.Security.Claims;
using LosPollos.Backend.Api.Cache;
using LosPollos.Backend.Api.Context;
using LosPollos.Backend.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace LosPollos.Backend.Api.Auth;

public class ActiveUserRequirement : IAuthorizationRequirement;

public class ActiveUserHandler : AuthorizationHandler<ActiveUserRequirement>
{
    private readonly IActivityCacheService _activityCache;
    private readonly ILogger<ActiveUserHandler> _logger;
    private readonly ApplicationContext _dbContext;

    public ActiveUserHandler(IActivityCacheService activityCache, ILogger<ActiveUserHandler> logger, ApplicationContext dbContext)
    {
        _activityCache = activityCache;
        _logger = logger;
        _dbContext = dbContext;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveUserRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated ?? false)
        {
            Claim userIdClaim = context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                try
                {
                    bool? isActive = await _activityCache.GetIsActiveStatusAsync(userId);
                    if (!isActive.HasValue)
                    {
                        User user = await _dbContext.Users.FirstAsync(user => user.Id == userId);
                        await _activityCache.SetIsActiveStatusAsync(userId, user.IsActive, TimeSpan.FromMinutes(30));
                        isActive = user.IsActive;
                    }
                    
                    if (isActive.Value)
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while getting user active status for user ID {UserId}", userId);
                }
            }
            else
            {
                _logger.LogWarning("Invalid user ID format: {UserId}", context.User.Identity.Name);
            }
        }
        
        context.Fail();
    }
}
