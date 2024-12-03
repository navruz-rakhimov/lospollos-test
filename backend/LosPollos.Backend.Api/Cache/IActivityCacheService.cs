namespace LosPollos.Backend.Api.Cache;

public interface IActivityCacheService
{
    Task SetIsActiveStatusAsync(int userId, bool isActive, TimeSpan expirationTime);
    Task<bool?> GetIsActiveStatusAsync(int userId);
}