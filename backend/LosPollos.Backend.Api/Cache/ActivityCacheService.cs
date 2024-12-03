using Microsoft.Extensions.Caching.Distributed;

namespace LosPollos.Backend.Api.Cache;

public class ActivityCacheService : IActivityCacheService
{
    // Use Redis or Memcached in production as a distributed caching solution
    private readonly IDistributedCache _cache;
    private const string Active = nameof(Active);
    private const string InActive = nameof(InActive);

    public ActivityCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    public async Task SetIsActiveStatusAsync(int userId, bool isActive, TimeSpan expirationTime)
    {
        string status = isActive ? Active : InActive;
        await _cache.SetStringAsync(userId.ToString(), status, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expirationTime
        });
    }

    public async Task<bool?> GetIsActiveStatusAsync(int userId)
    {
        string? status = await _cache.GetStringAsync(userId.ToString());
        return status?.Equals(Active, StringComparison.OrdinalIgnoreCase);
    }
}